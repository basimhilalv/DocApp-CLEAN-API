using DoctorBookingApp.Application.DTOs.DoctorDto;
using DoctorBookingApp.Application.DTOs.TimeSlotDto;
using DoctorBookingApp.Domain.Entities;

namespace DoctorBookingApp.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<string> CreateDoctorProfile(Guid userId, DoctorReqDto request);
        Task<string> UpdateDoctorProfile(Guid userId, DoctorReqDto request);
        Task<string> DeleteDoctorProfile(Guid userId);
        Task<DoctorResDto> GetDoctorProfile(Guid userId);
        Task<string> GenerateTimeSlot(Guid userId, SetScheduleDto request);
        Task<IEnumerable<TimeSlot>> GetAllTimeSlots(Guid userId);  
        Task<string> RemoveOneTimeSlot(Guid userId, Guid slotId);
        Task<string> RemoveAllTimeSlot(Guid userId);
        Task<string> RemoveTimeSlotByDate(Guid userId, DateTime date);
        Task<IEnumerable<Appointment>> GetAllApointments(Guid userId);

    }
}
