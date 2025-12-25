using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Services.Commands.DeactivateService;

public class DeactivateServiceCommandHandler : IRequestHandler<DeactivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeactivateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null)
        {
            throw new KeyNotFoundException($"Service with ID {request.ServiceId} not found.");
        }

        service.Deactivate();
        await _serviceRepository.UpdateAsync(service);
        await _unitOfWork.SaveChangesAsync();
    }
}
