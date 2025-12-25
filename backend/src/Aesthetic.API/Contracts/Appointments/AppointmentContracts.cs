namespace Aesthetic.API.Contracts.Appointments
{
    public record BookAppointmentRequest(
        Guid ServiceId,
        DateTime StartTime
    );

    public record AppointmentResponse(
        Guid Id,
        Guid CustomerId,
        Guid ProfessionalId,
        Guid ServiceId,
        DateTime StartTime,
        DateTime EndTime,
        decimal Price,
        string Status
    );
}