using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DoctorBookingApp.Application.Interfaces.Services;
using DoctorBookingApp.Domain.Entities;
using DoctorBookingApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Net;
using DoctorBookingApp.Infrastructure.SignalR;
using DoctorBookingApp.Application.DTOs.PatientDto;
using Twilio.Jwt.AccessToken;
using Microsoft.Extensions.Options;
using DoctorBookingApp.Infrastructure.Settings;
using Token = Twilio.Jwt.AccessToken.Token;

namespace DoctorBookingApp.Infrastructure.Services.PatientService
{
    public class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly IHubContext<BookingHub> _hubContext;
        private readonly IOptions<TwilioSetttings> _twiliosettings;

        public PatientService(
            IMapper mapper,
            AppDbContext context,
            Cloudinary cloudinary,
            IHubContext<BookingHub> hubContext,
            IOptions<TwilioSetttings> twiliosettings)
        {
            _mapper = mapper;
            _context = context;
            _cloudinary = cloudinary;
            _hubContext = hubContext;
            _twiliosettings = twiliosettings;
        }

        public async Task<string> CreateStripePaymentIntent(Guid appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a=>a.Id == appointmentId && a.PaymentStatus == null);
                if (appointment == null) throw new Exception("Appointment Not Found");

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(appointment.FeeAmount * 100),
                    Currency = "inr",
                    PaymentMethodTypes = new List<string> { "card" },
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                appointment.StripePaymentIntentId = paymentIntent.Id;
                appointment.PaymentStatus = "Pending";

                await _context.SaveChangesAsync();
                return paymentIntent.ClientSecret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> MarkAppointmentAsPaid(string paymentIntentId, string transactionId)
        {
            try
            {
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a=>a.StripePaymentIntentId == paymentIntentId && a.PaymentStatus == "Pending");
                if (appointment is null) throw new Exception("Appointment not found with stripe details");
                appointment.PaymentStatus = "Paid";
                appointment.StripeTransactionId = transactionId;

                await _context.SaveChangesAsync();
                return "Payment marked as paid";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> MarkAppointmentAsCash(Guid appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId && a.PaymentStatus == null);
                if (appointment is null) throw new Exception("Appointment is not found");
                appointment.PaymentStatus = "Cash";
                await _context.SaveChangesAsync();
                return "Payment marked as Cash";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> CancelAppointment(Guid userId, Guid appointmentId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (patient == null) throw new Exception("Patient profile not available");
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == patient.Id);
                if (appointment == null) throw new Exception("Appointment is not found");
                var slot = await _context.TimeSlots.FirstOrDefaultAsync(t => t.Id == appointment.TimeSlotId);
                if (slot == null) throw new Exception("Time Slot is not found");
                appointment.Status = "Cancelled";
                slot.IsBooked = false;
                await _context.SaveChangesAsync();
                return "Appointment Cancelled Successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> CreatePatientProfile(Guid userId, PatientReqDto request)
        {
            try
            {
                if (request is null) throw new Exception("Please provide profile details");
                var profileexist = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (profileexist != null) throw new Exception("Patient profile is already exists");
                var profile = _mapper.Map<Patient>(request);
                profile.UserId = userId;

                if(request.Avatar != null && request.Avatar.Length > 0)
                {
                    await using var stream = request.Avatar.OpenReadStream();

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(request.Avatar.FileName, stream),
                        Folder = "patient_avatars"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if(uploadResult.StatusCode == HttpStatusCode.OK)
                    {
                        profile.AvatarURL = uploadResult.SecureUrl.ToString();
                    }
                    else
                    {
                        throw new Exception("Image Upload Failed");
                    }
                }

                _context.Patients.Add(profile);
                await _context.SaveChangesAsync();
                return "Profile is created successfully";
                
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeletePatientProfile(Guid userId)
        {
            try
            {
                var profile = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if(profile != null)
                {
                    _context.Patients.Remove(profile);
                    await _context.SaveChangesAsync();
                    return "Profile Deleted Successfully";
                }
                return "Profile data deosn't exists";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Appointment>> GetAppointments(Guid userId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p=> p.UserId == userId);
                if (patient is null) throw new Exception("Patient profile not available");
                var appointments = await _context.Appointments.Where(a => a.PatientId == patient.Id).ToListAsync();
                if (!appointments.Any()) throw new Exception("No Appointments available");
                return appointments;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Doctor>> GetAvailableDoctors()
        {
            try
            {
                var doctors = await _context.Doctors.Where(d=>d.IsVerified == true).ToListAsync();
                if (doctors.Count == 0) throw new Exception("No Doctors available");
                return doctors;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PatientResDto> GetPatientProfile(Guid userId)
        {
            try
            {
                var profile = await _context.Patients.Include(p=>p.User).FirstOrDefaultAsync(p => p.UserId == userId);
                if (profile is null) throw new Exception("Profile data deosn't exists");
                var profileRes = _mapper.Map<PatientResDto>(profile);
                return profileRes;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TimeSlot>> GetTimeSlots(Guid doctorId)
        {
            try
            {
                var timeslots = await _context.TimeSlots.Where(t=> t.DoctorId == doctorId && t.SlotDate >= DateTime.UtcNow).ToListAsync();
                if (timeslots.Count == 0) throw new Exception("No TimeSlots available");
                return timeslots;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> MakeAppointment(Guid userId, Guid slotId)
        {
            var slot = await _context.TimeSlots.FirstOrDefaultAsync(t => t.Id == slotId);
            if (slot == null || slot.IsBooked) throw new Exception("Time Slot is not available");

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null) throw new Exception("Profile Data Deosn't Exist");

            //Begin Transaction to avoid race conditions.
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                slot.IsBooked = true;
                var appointment = new Appointment
                {
                    PatientId = patient.Id,
                    DoctorId = slot.DoctorId,
                    TimeSlotId = slot.Id,
                    AppointmentDate = slot.SlotDate,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Status = "Booked"
                };
                _context.Appointments.Add(appointment);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                //broadcast booking updates using signalR
                await _hubContext.Clients.Group(slot.DoctorId.ToString()).SendAsync("SlotBooked", slot.Id);
                return "Appointment Booked Successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdatePatientProfile(Guid userId, PatientReqDto request)
        {
            try
            {
                var profile = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (profile is null) throw new Exception("Profile Data Deosn't exist");
                if (request.Avatar != null && request.Avatar.Length > 0)
                {
                    await using var stream = request.Avatar.OpenReadStream();

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(request.Avatar.FileName, stream),
                        Folder = "patient_avatars"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == HttpStatusCode.OK)
                    {
                        profile.AvatarURL = uploadResult.SecureUrl.ToString();
                    }
                    else
                    {
                        throw new Exception("Image Upload Failed");
                    }
                }
                profile.FullName = request.FullName;
                profile.Age = request.Age;
                profile.Gender = request.Gender;
                profile.DateOfBirth = request.DateOfBirth;
                profile.Phone = request.Phone;
                profile.Address = request.Address;
                profile.BloodGroup = request.BloodGroup;
                profile.Height = request.Height;
                profile.Weight = request.Weight;
                profile.KnownAllergies = request.KnownAllergies;
                profile.ExistingConditions = request.ExistingConditions;
                profile.MedicalHistoryNotes = request.MedicalHistoryNotes;
                profile.Medications = request.Medications;
                profile.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return "Profile Updated Successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GenerateVideoToken(Guid userId, Guid appointmentId)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (patient == null) throw new Exception("Patient profile not available");
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == patient.Id);
                if (appointment == null) throw new Exception("Appointment is not found");
                if (appointment.PaymentStatus != "Paid") throw new Exception("Payment is not done");

                var now = DateTime.UtcNow;
                Console.WriteLine(now);
                var ISTDate = TimeZoneInfo.ConvertTimeFromUtc(now, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                Console.WriteLine(ISTDate);
                if (appointment.StartTime > ISTDate.TimeOfDay || appointment.EndTime < ISTDate.TimeOfDay) throw new Exception("Appointment time not yet started or has passed");

                var identity = userId.ToString();
                var roomName = appointmentId.ToString();

                var videoGrant = new VideoGrant { Room =  roomName };
                var grants = new HashSet<IGrant> { videoGrant };

                var token = new Token(
                    _twiliosettings.Value.AccountSid,
                    _twiliosettings.Value.ApiKey,
                    _twiliosettings.Value.ApiSecret,
                    identity: identity,
                    grants: grants
                    );

                return token.ToJwt();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
