using Aesthetic.Domain.Entities;

namespace Aesthetic.Domain.Interfaces
{
    public interface IProfessionalRepository : IRepository<Professional>
    {
        Task<Professional?> GetByUserIdAsync(Guid userId);
        Task<Professional?> GetByStripeAccountIdAsync(string stripeAccountId);
        Task<IEnumerable<Professional>> GetBySpecialtyAsync(string specialty);
    }
}