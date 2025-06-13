using DoctorBookingApp.Domain.Entities;

namespace DoctorBookingApp.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<string> BlockOrUnblock(Guid userId);
        Task<IEnumerable<Doctor>> GetDoctorsList();
        Task<IEnumerable<Patient>> GetPatientList();
        Task<string> VerifyDoctor(Guid DoctorId);
        Task<string> BlockDoctor(Guid DoctorId);
    }
}
