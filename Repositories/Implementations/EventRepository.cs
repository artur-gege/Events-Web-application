using ModsenAPI.Models;
using ModsenAPI.Data;
using Microsoft.EntityFrameworkCore;
using ModsenAPI.Repositories.Interfaces;

namespace ModsenAPI.Repositories.Implementations
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync(int page, int pageSize)
        {
            return await _context.Events
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<Event?> GetEventByNameAsync(string name)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<Event> CreateEventAsync(Event eventToCreate)
        {
            _context.Events.Add(eventToCreate);
            await _context.SaveChangesAsync();
            return eventToCreate;
        }

        public async Task<Event> UpdateEventAsync(Event eventToUpdate)
        {
            _context.Entry(eventToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return eventToUpdate;
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);

            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetEventsByCriteriasAsync(DateTime? date, string? location, string? category)
        {
            var query = _context.Events.AsQueryable();

            if (date != null)
            {
                query = query.Where(e => e.DateTime == date);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location == location);
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }

            return await query.ToListAsync();
        }

        public async Task<Event?> UpdateImageUrlAsync(int eventId, string imageUrl)
        {
            var eventToUpdate = await _context.Events.FindAsync(eventId);

            if (eventToUpdate != null)
            {
                eventToUpdate.ImageUrl = imageUrl;
                await _context.SaveChangesAsync();
            }

            return eventToUpdate;
        }
    }
}
