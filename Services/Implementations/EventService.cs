using ModsenAPI.UnitOfWorks;
using ModsenAPI.Repositories;
using ModsenAPI.Models;
using ModsenAPI.Services.Interfaces;

namespace ModsenAPI.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync(int page, int pageSize)
        {
            return await _unitOfWork.EventRepository.GetAllEventsAsync(page, pageSize);
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _unitOfWork.EventRepository.GetEventByIdAsync(id);
        }

        public async Task<Event?> GetEventByNameAsync(string name)
        {
            return await _unitOfWork.EventRepository.GetEventByNameAsync(name);
        }

        public async Task<Event> CreateEventAsync(Event eventToCreate)
        {
            return await _unitOfWork.EventRepository.CreateEventAsync(eventToCreate);
        }

        public async Task<Event> UpdateEventAsync(Event eventToUpdate)
        {
            return await _unitOfWork.EventRepository.UpdateEventAsync(eventToUpdate);
        }

        public async Task DeleteEventAsync(int id)
        {
            await _unitOfWork.EventRepository.DeleteEventAsync(id);
        }

        public async Task<IEnumerable<Event>> GetEventsByCriteriasAsync(DateTime? date, string? location, string? category)
        {
            return await _unitOfWork.EventRepository.GetEventsByCriteriasAsync(date, location, category);
        }

        public async Task<Event?> UpdateImageUrlAsync(int eventId, string imageUrl)
        {
            return await _unitOfWork.EventRepository.UpdateImageUrlAsync(eventId, imageUrl);
        }
    }
}
