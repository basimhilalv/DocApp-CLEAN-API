using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DoctorBookingApp.Application.DTOs.DoctorDto
{
    public class DoctorReqDto
    {
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


        //upload files
        public IFormFile? Avatar { get; set; } //image
        [Required]
        public IFormFile? Certification { get; set; } //pdf/image

    }
}
