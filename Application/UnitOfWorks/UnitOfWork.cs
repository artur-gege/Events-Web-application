using ModsenAPI.Application.Repositories.Interfaces;
using ModsenAPI.Infrastructure.Data;
using ModsenAPI.Application.Repositories.Implementations;

namespace ModsenAPI.Application.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IEventRepository EventRepository { get; }
        public IParticipantRepository ParticipantRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            EventRepository = new EventRepository(_context);
            ParticipantRepository = new ParticipantRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
