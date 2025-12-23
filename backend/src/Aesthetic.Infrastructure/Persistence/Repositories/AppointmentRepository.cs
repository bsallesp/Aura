using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aesthetic.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AestheticDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _dbSet
                .Where(a => a.CustomerId == customerId)
                .Include(a => a.Professional)
                .Include(a => a.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(Guid professionalId)
        {
            return await _dbSet
                .Where(a => a.ProfessionalId == professionalId)
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet
                .Where(a => a.StartTime >= start && a.StartTime <= end)
                .ToListAsync();
        }
    }
}