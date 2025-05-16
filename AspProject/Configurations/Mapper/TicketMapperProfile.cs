using AspProject.Domain.Entities;
using AspProject.Domain.Models;
using AutoMapper;

namespace AspProject.Configurations.Mapper;

public class TicketMapperProfile : Profile
{
    public TicketMapperProfile()
    {
        CreateMap<TicketDto, Ticket>()
            .ForMember(dest => dest.TicketsSet, opt => opt.Ignore());
        
        CreateMap<Ticket, TicketDto>();
    }
}

