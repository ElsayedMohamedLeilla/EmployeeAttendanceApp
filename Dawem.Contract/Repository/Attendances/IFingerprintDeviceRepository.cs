using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Attendances;

namespace Dawem.Contract.Repository.Attendances
{
    public interface IFingerprintDeviceRepository : IGenericRepository<FingerprintDevice>
    {
        IQueryable<FingerprintDevice> GetAsQueryable(GetFingerprintDevicesCriteria criteria);
    }
}
