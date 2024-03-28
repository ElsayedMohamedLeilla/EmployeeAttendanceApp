using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Attendances.FingerprintDevices;

namespace Dawem.Contract.Repository.Attendances
{
    public interface IFingerprintDeviceRepository : IGenericRepository<FingerprintDevice>
    {
        IQueryable<FingerprintDevice> GetAsQueryable(GetFingerprintDevicesCriteria criteria);
    }
}
