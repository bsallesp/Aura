using Aesthetic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aesthetic.Infrastructure.Persistence.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.Price)
                .HasPrecision(18, 2);

            builder.HasOne(s => s.Professional)
                .WithMany(p => p.Services)
                .HasForeignKey(s => s.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}