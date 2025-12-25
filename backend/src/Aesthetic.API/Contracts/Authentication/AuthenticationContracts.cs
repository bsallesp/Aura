namespace Aesthetic.API.Contracts.Authentication
{
    public record RegisterRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string? Role = "Client",
        string? BusinessName = null
    );

    public record LoginRequest(
        string Email,
        string Password
    );

    public record AuthenticationResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Token
    );
}