using MediatR;

namespace Aesthetic.Application.Professionals.Commands.UpdateAvailability
{
    public record UpdateAvailabilityCommand(
        Guid ProfessionalId,
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime,
        bool IsDayOff
    ) : IRequest<Unit>;
}
