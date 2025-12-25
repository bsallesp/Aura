using Aesthetic.Domain.Entities;

namespace Aesthetic.Domain.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Appointment>> GetByProfessionalIdAsync(Guid professionalId);
        Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<bool> HasConflictAsync(Guid professionalId, DateTime startTime, DateTime endTime);
    }
}