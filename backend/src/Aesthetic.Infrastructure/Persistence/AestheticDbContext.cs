using Aesthetic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aesthetic.Infrastructure.Persistence
{
    public class AestheticDbContext : DbContext
    {
        public AestheticDbContext(DbContextOptions<AestheticDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AestheticDbContext).Assembly);
        }
    }
}