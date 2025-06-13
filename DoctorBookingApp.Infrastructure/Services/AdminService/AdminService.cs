using DoctorBookingApp.Application.Interfaces.Services;
using DoctorBookingApp.Domain.Entities;
using DoctorBookingApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorBookingApp.Infrastructure.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> BlockDoctor(Guid DoctorId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == DoctorId);
                if (doctor == null) throw new Exception("Doctor not found");
                doctor.IsVerified = false;
                await _context.SaveChangesAsync();
                return "Doctor profile is blocked successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> BlockOrUnblock(Guid userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) throw new Exception("User is not available");
                user.IsActive = !user.IsActive;
                await _context.SaveChangesAsync();
                return $"User active status changed to {user.IsActive}";
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsList()
        {
            try
            {
                var doctors = await _context.Doctors.ToListAsync();
                if (!doctors.Any()) throw new Exception("No doctors found");
                return doctors;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Patient>> GetPatientList()
        {
            try
            {
                var patients = await _context.Patients.ToListAsync();
                if (!patients.Any()) throw new Exception("No patients found");
                return patients;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                if (!users.Any()) throw new Exception("No Users found");
                return users;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> VerifyDoctor(Guid DoctorId)
        {
            try
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d=> d.Id == DoctorId);
                if (doctor == null) throw new Exception("Doctor not found");
                doctor.IsVerified = true;
                await _context.SaveChangesAsync();
                return "Doctor profile is verified successfully";
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
