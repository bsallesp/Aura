namespace Aesthetic.API.Contracts.Services
{
    public record CreateServiceRequest(
        string Name,
        decimal Price,
        int DurationMinutes,
        string? Description
    );

    public record ServiceResponse(
        Guid Id,
        Guid ProfessionalId,
        string Name,
        decimal Price,
        int DurationMinutes,
        string? Description,
        bool IsActive
    );
}