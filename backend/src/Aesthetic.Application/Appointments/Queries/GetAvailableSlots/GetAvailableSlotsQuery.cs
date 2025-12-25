using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetAvailableSlots
{
    public record GetAvailableSlotsQuery(
        Guid ProfessionalId,
        Guid ServiceId,
        DateTime Date
    ) : IRequest<List<DateTime>>;
}
