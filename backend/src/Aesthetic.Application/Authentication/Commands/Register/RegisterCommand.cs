using Aesthetic.Application.Services.Authentication;
using MediatR;

namespace Aesthetic.Application.Authentication.Commands.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password, string Role = "Client", string? BusinessName = null) : IRequest<AuthenticationResult>;
