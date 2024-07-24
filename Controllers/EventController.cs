using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModsenAPI.Models;
using ModsenAPI.Services.Interfaces;
using ModsenAPI.ModelsDTO;
using ModsenAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ModsenAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly IValidator<EventDto> _eventValidator;

        public EventController(IEventService eventService, IMapper mapper, IValidator<EventDto> eventValidator)
        {
            _eventService = eventService;
            _mapper = mapper;
            _eventValidator = eventValidator;
        }

        [HttpGet]
        [Route("events")]
        public async Task<IActionResult> GetAllEventsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var events = await _eventService.GetAllEventsAsync(page, pageSize);

            var eventsDto = _mapper.Map<List<EventDto>>(events);
            
            return Ok(eventsDto);
        }

        [HttpGet]
        [Route("events/{id}")]
        public async Task<IActionResult> GetEventByIdAsync(int id)
        {
            var _event = await _eventService.GetEventByIdAsync(id);

            if (_event == null)
            {
                return NotFound();
            }

            var eventDto = _mapper.Map<EventDto>(_event);

            return Ok(eventDto);
        }

        [HttpGet]
        [Route("events/name/{name}")]
        public async Task<IActionResult> GetEventByNameAsync(string name) 
        {
            var _event = await _eventService.GetEventByNameAsync(name);

            if (_event == null)
            {
                return NotFound();
            }

            var eventDto = _mapper.Map<EventDto>(_event);

            return Ok(eventDto);
        }

        [HttpPost]
        [Route("events")]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventDto newEventDto)
        {
            var validationResult = _eventValidator.Validate(newEventDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var newEvent = _mapper.Map<Event>(newEventDto);

            var createdEvent = await _eventService.CreateEventAsync(newEvent);

            var createdEventDto = _mapper.Map<EventDto>(createdEvent);

            return CreatedAtAction(nameof(GetEventByIdAsync), new { id = createdEvent.Id }, createdEventDto);
        }

        [HttpPut]
        [Route("events/{id}")]
        public async Task<IActionResult> UpdateEventAsync(int id, [FromBody] EventDto EventDto)
        {
            var EventFromDb = await _eventService.GetEventByIdAsync(id);

            if (EventFromDb == null)
            {
                return NotFound();
            }

            var validationResult = _eventValidator.Validate(EventDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _mapper.Map(EventDto, EventFromDb);

            var updatedEvent = await _eventService.UpdateEventAsync(EventFromDb);

            var updatedEventDto = _mapper.Map<EventDto>(updatedEvent);

            return Ok(updatedEventDto);
        }

        [HttpDelete]
        [Route("events/{id}")]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            var EventFromDb = await _eventService.GetEventByIdAsync(id);

            if (EventFromDb == null)
            {
                return NotFound();
            }

            await _eventService.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> GetEventsByCriteriasAsync([FromQuery] DateTime? date, [FromQuery] string? location, [FromQuery] string? category)
        {
            var events = await _eventService.GetEventsByCriteriasAsync(date, location, category);

            var eventsDto = _mapper.Map<List<EventDto>>(events);

            return Ok(eventsDto);
        }

        [HttpPut]
        [Route("events/{id}/image")]
        public async Task<IActionResult> UpdateImageUrlAsync(int id, string imageUrl)
        {
            var EventFromDb = await _eventService.UpdateImageUrlAsync(id, imageUrl);

            if (EventFromDb == null)
            {
                return NotFound();
            }

            var updatedEventDto = _mapper.Map<EventDto>(EventFromDb);

            return Ok(updatedEventDto);
        }
    }
    
}
