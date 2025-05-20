using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AutoMapper;

namespace AspProject.Configurations.Mapper;

public class TicketMapperProfile : Profile
{
    public TicketMapperProfile()
    {
        CreateMap<TicketsSet, TicketSetDto>()
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets));

        CreateMap<Ticket, TicketDto>();
        
        CreateMap<TicketDto, Ticket>();

        CreateMap<TicketSetDto, TicketsSet>()
            .ForMember(dest => dest.Tickets, opt => opt.Ignore()) 
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StudentId, opt => opt.Ignore());
    }
}

