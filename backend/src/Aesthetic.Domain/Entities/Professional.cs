using Aesthetic.Domain.Common;
using System;
using System.Collections.Generic;

namespace Aesthetic.Domain.Entities
{
    public class Professional : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string BusinessName { get; private set; }
        public string? Specialty { get; private set; }
        public string? Bio { get; private set; }
        public string? StripeAccountId { get; private set; }
        public bool IsStripeOnboardingCompleted { get; private set; }

        // Navigation properties
        public virtual User User { get; private set; } = null!;
        public virtual ICollection<Service> Services { get; private set; } = new List<Service>();
        public virtual ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public virtual ICollection<ProfessionalAvailability> Availabilities { get; private set; } = new List<ProfessionalAvailability>();

        protected Professional() 
        {
            BusinessName = null!;
            Specialty = null!;
            Bio = null!;
        }

        public Professional(Guid userId, string businessName, string? specialty, string? bio = null)
        {
            if (string.IsNullOrWhiteSpace(businessName)) throw new ArgumentException("Business name is required.", nameof(businessName));
            
            UserId = userId;
            BusinessName = businessName;
            Specialty = specialty;
            Bio = bio;
        }

        public void UpdateStripeAccountId(string stripeAccountId)
        {
            StripeAccountId = stripeAccountId;
        }

        public void CompleteStripeOnboarding()
        {
            IsStripeOnboardingCompleted = true;
        }

        public void UpdateAvailability(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, bool isDayOff)
        {
            var existingAvailability = Availabilities.FirstOrDefault(a => a.DayOfWeek == dayOfWeek);

            if (existingAvailability != null)
            {
                existingAvailability.Update(startTime, endTime, isDayOff);
            }
            else
            {
                Availabilities.Add(new ProfessionalAvailability(Id, dayOfWeek, startTime, endTime, isDayOff));
            }
        }
    }
}
