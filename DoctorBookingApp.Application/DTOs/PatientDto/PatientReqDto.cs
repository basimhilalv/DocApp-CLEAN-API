
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Application.DTOs.PatientDto
{
    public class PatientReqDto
    {
        //Personal Data
        [Required]
        public string? FullName { get; set; }
        [Required]
        public int Age { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Address { get; set; }

        //Medical data
        public string? BloodGroup { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string? KnownAllergies { get; set; }
        public string? ExistingConditions { get; set; }
        public string? Medications { get; set; }
        public string? MedicalHistoryNotes { get; set; }

        //Profile Image
        public IFormFile? Avatar { get; set; }
    }
}
