namespace Aesthetic.API.Contracts.Users
{
    public record UserResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email
    );
}
