using ModsenAPI.Data;
using ModsenAPI.Repositories.Implementations;
using ModsenAPI.Repositories.Interfaces;

namespace ModsenAPI.UnitOfWorks
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
