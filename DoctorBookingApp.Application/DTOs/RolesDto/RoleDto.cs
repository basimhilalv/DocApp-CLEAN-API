using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Application.DTOs.RolesDto
{
    public class RoleDto
    {
        [Required]
        public string? Name { get; set; }

    }
}
