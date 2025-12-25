using Aesthetic.Domain.Interfaces;
using Aesthetic.Application.Common.Interfaces.Security;
using MediatR;

namespace Aesthetic.Application.Services.Commands.UpdateServicePolicies;

public class UpdateServicePoliciesCommandHandler : IRequestHandler<UpdateServicePoliciesCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;
    private readonly IProfessionalRepository _professionalRepository;

    public UpdateServicePoliciesCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork, IAuditService auditService, IProfessionalRepository professionalRepository)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
        _auditService = auditService;
        _professionalRepository = professionalRepository;
    }

    public async Task Handle(UpdateServicePoliciesCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null) throw new KeyNotFoundException("Service not found.");

        var professional = await _professionalRepository.GetByUserIdAsync(request.ActorUserId);
        if (professional == null || service.ProfessionalId != professional.Id)
        {
            throw new UnauthorizedAccessException("Only the owning professional can update service policies.");
        }

        service.UpdatePolicies(request.DepositPercentage, request.CancelFeePercentage, request.CancelFeeWindowHours);
        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogAsync(request.ActorUserId, "Service.UpdatePolicies", "Service", request.ServiceId,
            $"{{\"DepositPercentage\":{request.DepositPercentage?.ToString() ?? "null"},\"CancelFeePercentage\":{request.CancelFeePercentage?.ToString() ?? "null"},\"CancelFeeWindowHours\":{request.CancelFeeWindowHours?.ToString() ?? "null"}}}");
    }
}
