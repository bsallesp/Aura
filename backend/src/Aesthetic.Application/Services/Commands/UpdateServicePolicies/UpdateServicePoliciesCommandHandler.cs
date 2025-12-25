using Aesthetic.Domain.Interfaces;
using Aesthetic.Application.Common.Interfaces.Security;
using MediatR;

namespace Aesthetic.Application.Services.Commands.UpdateServicePolicies;

public class UpdateServicePoliciesCommandHandler : IRequestHandler<UpdateServicePoliciesCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _auditService;

    public UpdateServicePoliciesCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork, IAuditService auditService)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
        _auditService = auditService;
    }

    public async Task Handle(UpdateServicePoliciesCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null) throw new KeyNotFoundException("Service not found.");

        service.UpdatePolicies(request.DepositPercentage, request.CancelFeePercentage, request.CancelFeeWindowHours);
        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.SaveChangesAsync();

        await _auditService.LogAsync(request.ActorUserId, "Service.UpdatePolicies", "Service", request.ServiceId,
            $"{{\"DepositPercentage\":{request.DepositPercentage?.ToString() ?? "null"},\"CancelFeePercentage\":{request.CancelFeePercentage?.ToString() ?? "null"},\"CancelFeeWindowHours\":{request.CancelFeeWindowHours?.ToString() ?? "null"}}}");
    }
}
