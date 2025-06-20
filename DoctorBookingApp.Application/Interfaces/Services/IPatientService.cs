using DoctorBookingApp.Application.DTOs.PatientDto;
using DoctorBookingApp.Domain.Entities;


namespace DoctorBookingApp.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<string> CreatePatientProfile(Guid userId, PatientReqDto request);
        Task<string> UpdatePatientProfile(Guid userId, PatientReqDto request);
        Task<string> DeletePatientProfile(Guid userId);
        Task<PatientResDto> GetPatientProfile(Guid userId);
        Task<IEnumerable<Doctor>> GetAvailableDoctors();
        Task<string> MakeAppointment(Guid userId, Guid slotId);
        Task<string> CancelAppointment(Guid userId, Guid appointmentId);
        Task<IEnumerable<Appointment>> GetAppointments(Guid userId);
        Task<IEnumerable<TimeSlot>> GetTimeSlots(Guid doctorId);
        Task<string> CreateStripePaymentIntent(Guid appointmentId);
        Task<string> MarkAppointmentAsPaid(string paymentIntentId, string transactionId);
        Task<string> MarkAppointmentAsCash(Guid appointmentId);
        Task<string> GenerateVideoToken(Guid userId, Guid appointmentId);
        Task<IEnumerable<Message>> GetMessage(Guid userId1, Guid userId2);
        Task<Message> SendMessage(Guid userId1, Guid userId2, string message);
        Task<IEnumerable<Doctor>> GetDoctorByName(string  doctorName);
    }
}
