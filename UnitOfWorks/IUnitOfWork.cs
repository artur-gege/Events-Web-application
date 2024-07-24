using ModsenAPI.Repositories.Interfaces;

namespace ModsenAPI.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        IParticipantRepository ParticipantRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
