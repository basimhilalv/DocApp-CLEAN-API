using AutoMapper;
using DoctorBookingApp.Application.DTOs.DoctorDto;
using DoctorBookingApp.Application.DTOs.PatientDto;
using DoctorBookingApp.Application.DTOs.UserDto;
using DoctorBookingApp.Domain.Entities;

namespace DoctorBookingApp.Infrastructure.Persistence.Mappings
{
    public class AppMappings : Profile
    {
        public AppMappings()
        {
            CreateMap<User, UserRegDto>().ReverseMap();
            CreateMap<User, UserRegResDto>().ReverseMap();
            CreateMap<Patient, PatientReqDto>().ReverseMap();
            CreateMap<Patient, PatientResDto>().ReverseMap();
            CreateMap<Doctor, DoctorReqDto>().ReverseMap();
            CreateMap<Doctor, DoctorResDto>().ReverseMap();
        }
    }
}
