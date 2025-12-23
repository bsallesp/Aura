using Aesthetic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aesthetic.Infrastructure.Persistence.Configurations
{
    public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
    {
        public void Configure(EntityTypeBuilder<Professional> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.BusinessName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Bio)
                .HasMaxLength(1000);

            builder.Property(p => p.StripeAccountId)
                .HasMaxLength(100);

            // 1:N Relationship Professional -> Services
            builder.HasMany(p => p.Services)
                .WithOne(s => s.Professional)
                .HasForeignKey(s => s.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N Relationship Professional -> Appointments
            builder.HasMany(p => p.Appointments)
                .WithOne(a => a.Professional)
                .HasForeignKey(a => a.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
