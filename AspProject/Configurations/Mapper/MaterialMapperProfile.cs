using AspProject.Api.Models;
using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AutoMapper;

namespace AspProject.Configurations.Mapper;

public class MaterialMapperProfile : Profile
{
    
    public MaterialMapperProfile()
    {
        CreateMap<AddNewMaterialRequest, MaterialDto>()
            .ForMember(dest => dest.MaterialId, 
                opt => opt.Ignore())
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject ?? string.Empty))
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName ?? string.Empty))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Files, opt => opt.Ignore()); 
        
        CreateMap<AddNewMaterialRequest, MaterialDto>().ReverseMap();
        
        CreateMap<MaterialDto, Material>();
        
        CreateMap<Material, MaterialResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest.MaterialId, opt => opt.MapFrom(src => src.MaterialId));

        
        CreateMap<MaterialResponseDto, MaterialResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MaterialId))
            .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => src.TeacherName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.ImagesNameUrl, opt => opt.Ignore())
            .ForMember(dest => dest.FileNameUrl, opt => opt.Ignore());
        
        CreateMap<Material, MaterialResponseDto>();
        
    }
}
