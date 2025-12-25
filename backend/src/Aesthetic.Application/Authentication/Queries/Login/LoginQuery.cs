using Aesthetic.Application.Services.Authentication;
using MediatR;

namespace Aesthetic.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<AuthenticationResult>;
