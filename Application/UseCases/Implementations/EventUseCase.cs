using ModsenAPI.Domain.Models;
using ModsenAPI.Application.UseCases.Interfaces;
using ModsenAPI.Application.UnitOfWorks;
using AutoMapper;
using FluentValidation;
using ModsenAPI.Application.CustomExceptions;
using ModsenAPI.Domain.ModelsDTO.EventDTO;

namespace ModsenAPI.Application.UseCases.Implementations
{
    public class EventUseCase : IEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventRequestDto> _eventValidator;

        public EventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EventRequestDto> eventValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _eventValidator = eventValidator;
        }

        public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync(int page, int pageSize)
        {
            var events = await _unitOfWork.EventRepository.GetAllEventsAsync(page, pageSize);
            return events.Select(e => new EventResponseDto(e));
        }

        public async Task<int> GetTotalEventsAsync()
        {
            return await _unitOfWork.EventRepository.GetTotalEventsAsync();
        }

        public async Task<EventResponseDto?> GetEventByIdAsync(int id)
        {
            var currentEvent = await _unitOfWork.EventRepository.GetEventByIdAsync(id);
            
            if (currentEvent == null)
            {
                throw new NotFoundException("Событие с таким id не найдено");
            }

            var eventResponseDto = _mapper.Map<EventResponseDto>(currentEvent);

            return eventResponseDto;
        }

        public async Task<EventResponseDto?> GetEventByNameAsync(string name)
        {
            var currentEvent = await _unitOfWork.EventRepository.GetEventByNameAsync(name);

            if (currentEvent == null)
            {
                throw new NotFoundException("Событие с таким именем не найдено");
            }

            var eventResponseDto = _mapper.Map<EventResponseDto>(currentEvent);

            return eventResponseDto;
        }

        public async Task<EventResponseDto> CreateEventAsync(EventRequestDto eventToCreate)
        {
            var validationResult = _eventValidator.Validate(eventToCreate);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingEvent = await _unitOfWork.EventRepository.GetEventByNameAsync(eventToCreate.Name);
            if (existingEvent != null)
            {
                throw new AlreadyExistsException("Событие с таким именем уже существует");
            }

            var newEvent = _mapper.Map<Event>(eventToCreate);

            var createdEvent = await _unitOfWork.EventRepository.CreateEventAsync(newEvent);

            await _unitOfWork.SaveChangesAsync();

            var createdEventResponseDto = _mapper.Map<EventResponseDto>(createdEvent);

            return createdEventResponseDto;
        }

        public async Task<EventResponseDto> UpdateEventAsync(int id, EventRequestDto eventToUpdate)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                throw new NotFoundException("Событие с таким id не существует");
            }

            var validationResult = _eventValidator.Validate(eventToUpdate);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var newEvent = _mapper.Map<Event>(eventToUpdate);

            var updatedEvent = await _unitOfWork.EventRepository.UpdateEventAsync(newEvent);
            await _unitOfWork.SaveChangesAsync();

            var updatedEventResponseDto = _mapper.Map<EventResponseDto>(updatedEvent);

            return updatedEventResponseDto;
        }

        public async Task DeleteEventAsync(int id)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                throw new NotFoundException("Событие с таким id не существует");
            }

            await _unitOfWork.EventRepository.DeleteEventAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventResponseDto>> GetEventsByCriteriasAsync(DateTime? date, string? location, string? category)
        {
            var events = await _unitOfWork.EventRepository.GetEventsByCriteriasAsync(date, location, category);

            var eventsResponseDto = _mapper.Map<List<EventResponseDto>>(events);

            return eventsResponseDto;
        }

        public async Task<EventResponseDto?> UpdateImageUrlAsync(int eventId, string imageUrl)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetEventByIdAsync(eventId);
            if (existingEvent == null)
            {
                throw new NotFoundException("Событие с таким id не существует");
            }

            var _event = await _unitOfWork.EventRepository.UpdateImageUrlAsync(eventId, imageUrl);

            await _unitOfWork.SaveChangesAsync();

            var updatedEventResponseDto = _mapper.Map<EventResponseDto>(_event);

            return updatedEventResponseDto;
        }
    }
}
