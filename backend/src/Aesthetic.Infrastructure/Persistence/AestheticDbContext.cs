using Aesthetic.Domain.Common;
using Aesthetic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aesthetic.Infrastructure.Persistence
{
    public class AestheticDbContext : DbContext
    {
        private readonly IPublisher? _publisher;

        public AestheticDbContext(DbContextOptions<AestheticDbContext> options, IPublisher? publisher = null) : base(options)
        {
            _publisher = publisher;
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_publisher != null)
            {
                await DispatchDomainEventsAsync();
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task DispatchDomainEventsAsync()
        {
            var domainEventEntities = ChangeTracker.Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            foreach (var entity in domainEventEntities)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    await _publisher!.Publish(domainEvent);
                }
            }
        }
    }
}