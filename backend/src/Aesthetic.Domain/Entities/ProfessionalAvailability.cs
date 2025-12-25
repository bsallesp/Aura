using Aesthetic.Domain.Common;

namespace Aesthetic.Domain.Entities
{
    public class ProfessionalAvailability : BaseEntity
    {
        public Guid ProfessionalId { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public bool IsDayOff { get; private set; }
        
        // Navigation
        public virtual Professional Professional { get; private set; } = null!;

        protected ProfessionalAvailability() { }

        public ProfessionalAvailability(Guid professionalId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, bool isDayOff = false)
        {
            if (startTime >= endTime && !isDayOff)
            {
                throw new ArgumentException("Start time must be before end time.");
            }

            ProfessionalId = professionalId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            IsDayOff = isDayOff;
        }

        public void Update(TimeSpan startTime, TimeSpan endTime, bool isDayOff)
        {
            if (startTime >= endTime && !isDayOff)
            {
                throw new ArgumentException("Start time must be before end time.");
            }

            StartTime = startTime;
            EndTime = endTime;
            IsDayOff = isDayOff;
        }
    }
}
