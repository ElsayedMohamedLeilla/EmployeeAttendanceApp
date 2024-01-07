using Dawem.Data;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;

namespace Dawem.Contract.Repository.Employees
{
    public interface INonComplianceActionRepository : IGenericRepository<NonComplianceAction>
    {
        IQueryable<NonComplianceAction> GetAsQueryable(GetNonComplianceActionsCriteria criteria);
    }
}
