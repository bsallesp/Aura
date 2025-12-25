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

            builder.Property(p => p.UserId)
                .IsRequired();

            builder.Property(p => p.BusinessName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Bio)
                .HasMaxLength(1000);

            builder.Property(p => p.StripeAccountId)
                .HasMaxLength(100);
        }
    }
}