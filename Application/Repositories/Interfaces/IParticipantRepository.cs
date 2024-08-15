using ModsenAPI.Domain.Models;

namespace ModsenAPI.Application.Repositories.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant> AddParticipantAsync(Participant participantModel);
        Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
        Task<Participant?> GetParticipantByIdAsync(int id);
        Task DeleteParticipantAsync(int id);
    }
}
