using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Application.DTOs.UserDto
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

    }
}
