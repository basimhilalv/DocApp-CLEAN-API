using DoctorBookingApp.Domain.Entities;

namespace DoctorBookingApp.Application.DTOs.DoctorDto
{
    public class DoctorResDto
    {
        public Guid Id { get; set; }

        //Foreign Key To user table
        public Guid UserId { get; set; }
        public User User { get; set; }

        //Personal infor
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        //Professional infor
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public string? RegisterationNumber { get; set; }
        public int YearOfExperience { get; set; }
        public string? HospitalName { get; set; }
        public string? About { get; set; }
        public string? LanguagesSpoken { get; set; }

        public ICollection<TimeSlot>? TimeSlots { get; set; }

        //upload files
        public string? AvatarURL { get; set; }
        public string? CertificationURL { get; set; }

        //Rating
        public double AverageRating { get; set; }

    }
}
