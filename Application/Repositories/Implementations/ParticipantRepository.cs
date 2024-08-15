using Microsoft.EntityFrameworkCore;
using ModsenAPI.Application.Repositories.Interfaces;
using ModsenAPI.Domain.Models;
using ModsenAPI.Infrastructure.Data;

namespace ModsenAPI.Application.Repositories.Implementations
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly ApplicationDbContext _context;

        public ParticipantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _context.Participants.Where(p => p.EventId == eventId).ToListAsync();
        }

        public async Task<Participant?> GetParticipantByIdAsync(int id)
        {
            return await _context.Participants.FindAsync(id);
        }

        public async Task<Participant> AddParticipantAsync(Participant participantToAdd)
        {
            await _context.Participants.AddAsync(participantToAdd);
            return participantToAdd;
        }

        public async Task DeleteParticipantAsync(int id)
        {
            var participantToDelete = await _context.Participants.FindAsync(id);

            if (participantToDelete != null)
            {
                _context.Participants.Remove(participantToDelete);
            }
        }

    }
}
