using Aesthetic.Application.Common.Interfaces.Authentication;
using Aesthetic.Application.Services.Authentication;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginQueryHandler(
        IUserRepository userRepository,
        ITokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            throw new Exception("Invalid credentials.");
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid credentials.");
        }

        var token = _tokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
