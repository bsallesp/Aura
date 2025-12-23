using Aesthetic.Domain.Common;
using Aesthetic.Domain.Enums;
using System;

namespace Aesthetic.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid CustomerId { get; private set; } // Could be nullable if we allow guest checkout, but adhering to User entity for now
        public Guid ProfessionalId { get; private set; }
        public Guid ServiceId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public decimal PriceAtBooking { get; private set; }
        public string? StripePaymentIntentId { get; private set; }

        // Navigation properties
        public virtual User Customer { get; private set; } = null!;
        public virtual Professional Professional { get; private set; } = null!;
        public virtual Service Service { get; private set; } = null!;

            protected Appointment() { }

            public Appointment(Guid customerId, Guid professionalId, Guid serviceId, DateTime startTime, int durationMinutes, decimal price)
        {
            if (startTime < DateTime.UtcNow) throw new ArgumentException("Cannot book appointment in the past.", nameof(startTime));
            if (durationMinutes <= 0) throw new ArgumentException("Duration must be positive.", nameof(durationMinutes));
            
            CustomerId = customerId;
            ProfessionalId = professionalId;
            ServiceId = serviceId;
            StartTime = startTime;
            EndTime = startTime.AddMinutes(durationMinutes);
            Status = AppointmentStatus.Pending;
            PriceAtBooking = price;
        }

        public void Confirm(string paymentIntentId)
        {
            if (Status != AppointmentStatus.Pending) throw new InvalidOperationException("Only pending appointments can be confirmed.");
            
            Status = AppointmentStatus.Confirmed;
            StripePaymentIntentId = paymentIntentId;
            UpdateTimestamp();
        }

        public void Cancel()
        {
            if (Status == AppointmentStatus.Completed) throw new InvalidOperationException("Cannot cancel a completed appointment.");
            
            Status = AppointmentStatus.Cancelled;
            UpdateTimestamp();
        }

        public void Complete()
        {
            if (Status != AppointmentStatus.Confirmed) throw new InvalidOperationException("Only confirmed appointments can be completed.");

            Status = AppointmentStatus.Completed;
            UpdateTimestamp();
        }
    }
}
