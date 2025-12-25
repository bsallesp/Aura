using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Service>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Service> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new Service(request.ProfessionalId, request.Name, request.Price, request.DurationMinutes, request.Description);
        
        await _serviceRepository.AddAsync(service);
        await _unitOfWork.SaveChangesAsync();

        return service;
    }
}
