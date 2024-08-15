using Microsoft.AspNetCore.Mvc;
using ModsenAPI.Application.UseCases.Interfaces;
using ModsenAPI.Domain.ModelsDTO.EventDTO;
using ModsenAPI.Infrastructure.Pagination;

namespace ModsenAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class EventController : ControllerBase
    {
        private readonly IEventUseCase _eventUseCase;

        public EventController(IEventUseCase eventUseCase)
        {
            _eventUseCase = eventUseCase;
        }

        [HttpGet]
        [Route("events")]
        public async Task<IActionResult> GetAllEventsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var totalEvents = await _eventUseCase.GetTotalEventsAsync();
            var events = await _eventUseCase.GetAllEventsAsync(page, pageSize);

            var pagedResult = new PagedResult<EventResponseDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                NextPage = page + 1,
                PrevPage = page - 1,
                TotalPages = (int)Math.Ceiling((double)totalEvents / pageSize),
                Items = events
            };

            return Ok(pagedResult);
        }


        [HttpGet]
        [Route("events/{id}")]
        public async Task<IActionResult> GetEventByIdAsync(int id)
        {
            var _event = await _eventUseCase.GetEventByIdAsync(id);
            return Ok(_event);
        }

        [HttpGet]
        [Route("events/name/{name}")]
        public async Task<IActionResult> GetEventByNameAsync(string name)
        {
            var _event = await _eventUseCase.GetEventByNameAsync(name);
            return Ok(_event);
        }

        [HttpPost]
        [Route("events")]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventRequestDto newEvent)
        {
            var createdEvent = await _eventUseCase.CreateEventAsync(newEvent);
            return CreatedAtAction(nameof(GetEventByIdAsync), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPut]
        [Route("events/{id}")]
        public async Task<IActionResult> UpdateEventAsync(int id, [FromBody] EventRequestDto EventDto)
        {
            var updatedEvent = await _eventUseCase.UpdateEventAsync(id, EventDto);
            return Ok(updatedEvent);
        }

        [HttpDelete]
        [Route("events/{id}")]
        public async Task<IActionResult> DeleteEventAsync(int id)
        {
            await _eventUseCase.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> GetEventsByCriteriasAsync([FromQuery] DateTime? date, [FromQuery] string? location, [FromQuery] string? category)
        {
            var events = await _eventUseCase.GetEventsByCriteriasAsync(date, location, category);
            return Ok(events);

        }

        [HttpPut]
        [Route("events/{id}/image")]
        public async Task<IActionResult> UpdateImageUrlAsync(int id, string imageUrl)
        {
            var updatedEvent = await _eventUseCase.UpdateImageUrlAsync(id, imageUrl);
            return Ok(updatedEvent);
        }
    }

}
