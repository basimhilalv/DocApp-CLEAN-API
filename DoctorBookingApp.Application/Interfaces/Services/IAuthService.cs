using DoctorBookingApp.Application.DTOs.UserDto;

namespace DoctorBookingApp.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<string> Register(UserRegDto request);
        public Task<UserLogResDto> Login(UserLoginDto request);
    }
}
