using ModsenAPI.Domain.Models;
using ModsenAPI.Application.UnitOfWorks;
using ModsenAPI.Application.UseCases.Interfaces;
using ModsenAPI.Domain.ModelsDTO.ParticipantDTO;
using ModsenAPI.Application.Validators;
using AutoMapper;
using FluentValidation;
using ModsenAPI.Domain.ModelsDTO.EventDTO;
using ModsenAPI.Application.CustomExceptions;

namespace ModsenAPI.Application.Services.Implementations
{
    public class ParticipantUseCase : IParticipantUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ParticipantRequestDto> _participantValidator;

        public ParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ParticipantRequestDto> participantValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _participantValidator = participantValidator;
        }

        public async Task<IEnumerable<ParticipantResponseDto>> GetParticipantsByEventIdAsync(int eventId)
        {
            var currentEvent = await _unitOfWork.EventRepository.GetEventByIdAsync(eventId);

            if (currentEvent == null)
            {
                throw new NotFoundException("Событие с таким id не найдено");
            }

            var participants = await _unitOfWork.ParticipantRepository.GetParticipantsByEventIdAsync(eventId);

            var participantsResponseDto = _mapper.Map<List<ParticipantResponseDto>>(participants);

            return participantsResponseDto;
        }


        public async Task<ParticipantResponseDto?> GetParticipantByIdAsync(int id)
        {
            var currentParticipant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(id);

            if (currentParticipant == null)
            {
                throw new NotFoundException("Участник с таким id не найдено");
            }

            var participantResponseDto = _mapper.Map<ParticipantResponseDto>(currentParticipant);

            return participantResponseDto;
        }

        public async Task<ParticipantResponseDto> AddParticipantAsync(int eventId, ParticipantRequestDto participantToAdd)
        {
            var currentEvent = await _unitOfWork.EventRepository.GetEventByIdAsync(eventId);

            if (currentEvent == null)
            {
                throw new NotFoundException("Событие с таким id не найдено");
            }

            var validationResult = _participantValidator.Validate(participantToAdd);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var newParticipant = _mapper.Map<Participant>(participantToAdd);

            newParticipant.EventId = eventId;

            var addedParticipant = await _unitOfWork.ParticipantRepository.AddParticipantAsync(newParticipant);

            await _unitOfWork.SaveChangesAsync();

            var participantResponseDto = _mapper.Map<ParticipantResponseDto>(addedParticipant);

            return participantResponseDto;
        }

        public async Task DeleteParticipantAsync(int id)
        {
            var currentParticipant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(id);

            if (currentParticipant == null)
            {
                throw new NotFoundException("Участник с таким id не найдено");
            }

            await _unitOfWork.ParticipantRepository.DeleteParticipantAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
