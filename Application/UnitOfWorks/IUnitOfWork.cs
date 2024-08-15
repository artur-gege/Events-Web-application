using ModsenAPI.Application.Repositories.Interfaces;

namespace ModsenAPI.Application.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        IParticipantRepository ParticipantRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
