using DoctorBookingApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Application.DTOs.PatientDto
{
    public class PatientResDto
    {
        public Guid Id { get; set; }

        //Foreign Key to User table
        public Guid UserId { get; set; }
        public User User { get; set; }

        //Personal Data
        public string? FullName { get; set; }
        public int Age { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
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
        public string? AvatarURL { get; set; }

    }
}
