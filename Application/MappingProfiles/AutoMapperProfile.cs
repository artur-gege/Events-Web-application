using AutoMapper;
using ModsenAPI.Domain.Models;
using ModsenAPI.Domain.ModelsDTO.ParticipantDTO;
using ModsenAPI.Domain.ModelsDTO.EventDTO;

namespace ModsenAPI.Application.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EventRequestDto, Event>().ReverseMap();
            CreateMap<EventResponseDto, Event>().ReverseMap();

            CreateMap<ParticipantRequestDto, Participant>().ReverseMap();
            CreateMap<ParticipantResponseDto, Participant>().ReverseMap();
        }
    }
}
