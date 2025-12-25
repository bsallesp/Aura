using Aesthetic.Domain.Interfaces;
using Aesthetic.Application.Common.Interfaces.Security;
using MediatR;

namespace Aesthetic.Application.Services.Commands.DeactivateService;

public class DeactivateServiceCommandHandler : IRequestHandler<DeactivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public DeactivateServiceCommandHandler(IServiceRepository serviceRepository, IProfessionalRepository professionalRepository, IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _serviceRepository = serviceRepository;
        _professionalRepository = professionalRepository;
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    public async Task Handle(DeactivateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null)
        {
            throw new KeyNotFoundException($"Service with ID {request.ServiceId} not found.");
        }
        var professional = await _professionalRepository.GetByUserIdAsync(request.ActorUserId);
        if (professional == null || service.ProfessionalId != professional.Id)
        {
            throw new UnauthorizedAccessException("Only the owning professional can deactivate this service.");
        }

        service.Deactivate();
        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogAsync(request.ActorUserId, "Service.Deactivate", "Service", request.ServiceId, null);
    }
}
