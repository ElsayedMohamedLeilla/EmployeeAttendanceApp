using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Attendances.FingerprintDevices;

namespace Dawem.Contract.Repository.Attendances
{
    public interface IFingerprintDeviceRepository : IGenericRepository<FingerprintDevice>
    {
        IQueryable<FingerprintDevice> GetAsQueryable(GetFingerprintDevicesCriteria criteria);
    }
}
