using ModsenAPI.Domain.Models;
using ModsenAPI.Domain.ModelsDTO;
using ModsenAPI.Domain.ModelsDTO.EventDTO;

namespace ModsenAPI.Application.UseCases.Interfaces
{
    public interface IEventUseCase
    {
        Task<IEnumerable<EventResponseDto>> GetAllEventsAsync(int page, int pageSize);
        Task<int> GetTotalEventsAsync();
        Task<EventResponseDto?> GetEventByIdAsync(int id);
        Task<EventResponseDto?> GetEventByNameAsync(string name);
        Task<EventResponseDto> CreateEventAsync(EventRequestDto eventToCreate);
        Task<EventResponseDto> UpdateEventAsync(int id, EventRequestDto eventToUpdate);
        Task DeleteEventAsync(int id);
        Task<IEnumerable<EventResponseDto>> GetEventsByCriteriasAsync(DateTime? date, string? location, string? category);
        Task<EventResponseDto?> UpdateImageUrlAsync(int eventId, string imageUrl);
    }
}
