using AutoMapper;
using ModsenAPI.Models;
using ModsenAPI.ModelsDTO;

namespace ModsenAPI.AutoMappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<EventDto, Event>().ReverseMap();

            CreateMap<ParticipantDto, Participant>().ReverseMap();
        }
    }
}
