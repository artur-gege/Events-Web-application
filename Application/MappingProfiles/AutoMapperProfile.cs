using AutoMapper;
using ModsenAPI.Domain.Models;
using ModsenAPI.Domain.ModelsDTO.ParticipantDTO;
using ModsenAPI.Domain.ModelsDTO.EventDTO;
using ModsenAPI.Domain.ModelsDTO.UserDTO;

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

            CreateMap<UserRegisterRequestDto, User>().ReverseMap();
            CreateMap<UserLoginRequestDto, User>().ReverseMap();
            CreateMap<UserResponseDto, User>().ReverseMap();
        }
    }
}
