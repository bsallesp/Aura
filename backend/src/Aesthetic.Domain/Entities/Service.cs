using Aesthetic.Domain.Common;
using System;
using System.Collections.Generic;

namespace Aesthetic.Domain.Entities
{
    public class Service : BaseEntity
    {
        public Guid ProfessionalId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int DurationMinutes { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation properties
        public virtual Professional Professional { get; private set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();

        protected Service()
        {
            Name = null!;
            Description = null!;
            IsActive = true;
        }

        public Service(Guid professionalId, string name, decimal price, int durationMinutes, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Service name is required.", nameof(name));
            if (price < 0) throw new ArgumentException("Price cannot be negative.", nameof(price));
            if (durationMinutes <= 0) throw new ArgumentException("Duration must be greater than zero.", nameof(durationMinutes));

            ProfessionalId = professionalId;
            Name = name;
            Price = price;
            DurationMinutes = durationMinutes;
            Description = description;
            IsActive = true;
        }

        public void UpdateDetails(string name, decimal price, int durationMinutes, string? description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Service name is required.", nameof(name));
            if (price < 0) throw new ArgumentException("Price cannot be negative.", nameof(price));
            if (durationMinutes <= 0) throw new ArgumentException("Duration must be greater than zero.", nameof(durationMinutes));

            Name = name;
            Price = price;
            DurationMinutes = durationMinutes;
            Description = description;
            UpdateTimestamp();
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdateTimestamp();
        }

        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();
        }
    }
}
