using Aesthetic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aesthetic.Infrastructure.Persistence.Configurations
{
    public class ProfessionalAvailabilityConfiguration : IEntityTypeConfiguration<ProfessionalAvailability>
    {
        public void Configure(EntityTypeBuilder<ProfessionalAvailability> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ProfessionalId)
                .IsRequired();

            builder.Property(a => a.DayOfWeek)
                .IsRequired();

            builder.Property(a => a.StartTime)
                .IsRequired();

            builder.Property(a => a.EndTime)
                .IsRequired();

            builder.HasOne(a => a.Professional)
                .WithMany(p => p.Availabilities)
                .HasForeignKey(a => a.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
