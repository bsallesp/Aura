namespace Aesthetic.API.Contracts.Professionals
{
    public record CreateProfessionalRequest(
        string BusinessName,
        string? Specialty,
        string? Bio
    );

    public record ProfessionalResponse(
        Guid UserId,
        string BusinessName,
        string? Specialty,
        string? Bio
    );
    public record UpdateAvailabilityRequest(
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime,
        bool IsDayOff
    );

    public record AvailabilityResponse(
        DayOfWeek DayOfWeek,
        TimeSpan StartTime,
        TimeSpan EndTime,
        bool IsDayOff
    );
}