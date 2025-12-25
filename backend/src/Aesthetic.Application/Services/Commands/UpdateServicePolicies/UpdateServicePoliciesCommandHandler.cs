using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Services.Commands.UpdateServicePolicies;

public class UpdateServicePoliciesCommandHandler : IRequestHandler<UpdateServicePoliciesCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServicePoliciesCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateServicePoliciesCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null) throw new KeyNotFoundException("Service not found.");

        service.UpdatePolicies(request.DepositPercentage, request.CancelFeePercentage, request.CancelFeeWindowHours);
        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.SaveChangesAsync();
    }
}
