using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Domain.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; }

        //Foreign Key To user table
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        //Personal infor
        [Required]
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Address { get; set; }

        //Professional infor
        [Required]
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        [Required]
        public string? RegisterationNumber { get; set; }
        public int YearOfExperience { get; set; }
        public string? HospitalName { get; set; }
        public string? About { get; set; }
        public string? LanguagesSpoken { get; set; }

        public ICollection<TimeSlot>? TimeSlots { get; set; }

        //upload files
        public string? AvatarURL { get; set; }
        [Required]
        public string? CertificationURL { get; set; }

        //Rating
        public double AverageRating { get; set; }

        //Date and time of creation
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        //verification
        public bool IsVerified { get; set; } = false;

    }
}
