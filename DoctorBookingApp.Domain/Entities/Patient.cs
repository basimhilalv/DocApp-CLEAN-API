using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }

        //Foreign Key to User table
        public Guid UserId { get; set; }
        public User User { get; set; }

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
        public string AvatarURL { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

    }
}
