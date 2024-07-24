using ModsenAPI.UnitOfWorks;
using ModsenAPI.Models;
using ModsenAPI.Services.Interfaces;

namespace ModsenAPI.Services.Implementations
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParticipantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Participant> AddParticipantAsync(Participant participantToAdd)
        {
            return await _unitOfWork.ParticipantRepository.AddParticipantAsync(participantToAdd);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _unitOfWork.ParticipantRepository.GetParticipantsByEventIdAsync(eventId);
        }

        public async Task<Participant?> GetParticipantByIdAsync(int id)
        {
            return await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(id);
        }

        public async Task DeleteParticipantAsync(int id)
        {
            await _unitOfWork.ParticipantRepository.DeleteParticipantAsync(id);
        }

    }
}
