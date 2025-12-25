using Aesthetic.Domain.Entities;

namespace Aesthetic.Domain.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<IEnumerable<Service>> GetByProfessionalIdAsync(Guid professionalId);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
    }
}