using ModsenAPI.Domain.Models;
using ModsenAPI.Domain.ModelsDTO.ParticipantDTO;

namespace ModsenAPI.Application.UseCases.Interfaces
{
    public interface IParticipantUseCase
    {
        Task<ParticipantResponseDto> AddParticipantAsync(int eventId, ParticipantRequestDto participantToAdd);
        Task<IEnumerable<ParticipantResponseDto>> GetParticipantsByEventIdAsync(int eventId);
        Task<ParticipantResponseDto?> GetParticipantByIdAsync(int id);
        Task DeleteParticipantAsync(int id);
    }
}
