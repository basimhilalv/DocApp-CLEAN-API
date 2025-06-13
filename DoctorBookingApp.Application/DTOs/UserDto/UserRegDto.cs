using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Application.DTOs.UserDto
{
    public class UserRegDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username must not exceed 20 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can only contain alphabets, numbers and special characters like . - _")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@$%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int RoleId { get; set; }
    }
}
