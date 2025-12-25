using Aesthetic.Application.Common.Interfaces.Authentication;
using Aesthetic.Application.Services.Authentication;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Enums;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IProfessionalRepository _professionalRepository;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IProfessionalRepository professionalRepository,
        ITokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _professionalRepository = professionalRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        if (await _userRepository.GetByEmailAsync(request.Email) is not null)
        {
            throw new Exception("User with given email already exists.");
        }

        var role = request.Role.ToLower() == "professional" ? UserRole.Professional : UserRole.Customer;

        // Create user
        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            _passwordHasher.HashPassword(request.Password),
            role
        );

        await _userRepository.AddAsync(user);

        if (role == UserRole.Professional)
        {
            var professional = new Professional(
                user.Id,
                request.BusinessName ?? $"{request.FirstName} {request.LastName}",
                "General",
                null
            );
            await _professionalRepository.AddAsync(professional);
        }

        await _unitOfWork.SaveChangesAsync();

        var token = _tokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
