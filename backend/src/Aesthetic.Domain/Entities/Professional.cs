using Aesthetic.Domain.Common;
using System;
using System.Collections.Generic;

namespace Aesthetic.Domain.Entities
{
    public class Professional : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string BusinessName { get; private set; }
        public string? Bio { get; private set; }
        public string? StripeAccountId { get; private set; }
        public bool IsStripeOnboardingCompleted { get; private set; }

        // Navigation properties
        public virtual User User { get; private set; } = null!;
        public virtual ICollection<Service> Services { get; private set; } = new List<Service>();
        public virtual ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();

        protected Professional() { }

        public Professional(Guid userId, string businessName, string? bio = null)
        {
            if (string.IsNullOrWhiteSpace(businessName)) throw new ArgumentException("Business name is required.", nameof(businessName));
            
            UserId = userId;
            BusinessName = businessName;
            Bio = bio;
        }

        public void SetStripeAccountId(string stripeAccountId)
        {
            if (string.IsNullOrWhiteSpace(stripeAccountId)) throw new ArgumentException("Stripe Account ID is required.", nameof(stripeAccountId));
            StripeAccountId = stripeAccountId;
        }

        public void CompleteStripeOnboarding()
        {
            IsStripeOnboardingCompleted = true;
        }
    }
}
