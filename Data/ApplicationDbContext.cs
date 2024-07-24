using Microsoft.EntityFrameworkCore;
using ModsenAPI.Models;


namespace ModsenAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public ApplicationDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений между моделями (если необходимо)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Participants)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventId);
        }
    }
}
