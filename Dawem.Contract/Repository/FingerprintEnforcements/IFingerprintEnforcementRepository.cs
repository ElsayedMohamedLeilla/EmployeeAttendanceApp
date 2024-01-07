using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;

namespace Dawem.Contract.Repository.Employees
{
    public interface IFingerprintEnforcementRepository : IGenericRepository<FingerprintEnforcement>
    {
        IQueryable<FingerprintEnforcement> GetAsQueryable(GetFingerprintEnforcementsCriteria criteria);
    }
}
