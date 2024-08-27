using Microsoft.EntityFrameworkCore;
using ModsenAPI.Domain.Models;

namespace ModsenAPI.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public ApplicationDbContext() { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureEventParticipantRelationship(modelBuilder);
        }

        //вынес конфигурацию в отдельный метод
        private void ConfigureEventParticipantRelationship(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Participants)
                .WithOne(p => p.Event)
                .HasForeignKey(p => p.EventId);

            modelBuilder.Entity<User>()
            .Property(u => u.UserName)
            .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.RefreshToken)
                .IsRequired();
        }
    }
}
