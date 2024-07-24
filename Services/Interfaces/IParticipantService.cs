using ModsenAPI.Models;

namespace ModsenAPI.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<Participant> AddParticipantAsync(Participant participantToAdd);
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task<Participant?> GetParticipantByIdAsync(int id);
        Task DeleteParticipantAsync(int id);
    }
}
