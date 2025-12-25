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
}