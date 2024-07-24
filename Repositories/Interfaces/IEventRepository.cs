using ModsenAPI.Models;

namespace ModsenAPI.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(int page, int pageSize);
        Task<Event?> GetEventByIdAsync(int id);
        Task<Event?> GetEventByNameAsync(string name);
        Task<Event> CreateEventAsync(Event eventToCreate);
        Task<Event> UpdateEventAsync(Event eventToUpdate);
        Task DeleteEventAsync(int id);
        Task<IEnumerable<Event>> GetEventsByCriteriasAsync(DateTime? date, string? location, string? category);
        Task<Event?> UpdateImageUrlAsync(int eventId, string imageUrl);
    }
}
