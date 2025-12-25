using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(request.UserId);
    }
}
