using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Professionals.Commands.CreateProfile;

public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Professional>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProfileCommandHandler(IProfessionalRepository professionalRepository, IUnitOfWork unitOfWork)
    {
        _professionalRepository = professionalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Professional> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        // Verify if profile already exists for user?
        var existingProfile = await _professionalRepository.GetByUserIdAsync(request.UserId);
        if (existingProfile != null)
        {
            throw new InvalidOperationException("Professional profile already exists for this user.");
        }

        var professional = new Professional(request.UserId, request.BusinessName, request.Specialty, request.Bio);
        
        await _professionalRepository.AddAsync(professional);
        await _unitOfWork.SaveChangesAsync();

        return professional;
    }
}
