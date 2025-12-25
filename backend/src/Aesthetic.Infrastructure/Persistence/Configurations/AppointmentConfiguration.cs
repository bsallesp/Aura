using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aesthetic.Infrastructure.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Status)
                .HasConversion<string>();

            builder.Property(a => a.PriceAtBooking)
                .HasPrecision(18, 2);

            builder.Property(a => a.CancellationFeeAmount)
                .HasPrecision(18, 2);

            builder.Property(a => a.CancelledAt);

            builder.HasOne(a => a.Customer)
                .WithMany(u => u.AppointmentsAsCustomer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Professional)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
