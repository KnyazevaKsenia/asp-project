using AspProject.Domain.Entities;
using AspProject.Domain.Models;

namespace AspProject.Configurations.Mapper;

using AutoMapper;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));
    }
}

