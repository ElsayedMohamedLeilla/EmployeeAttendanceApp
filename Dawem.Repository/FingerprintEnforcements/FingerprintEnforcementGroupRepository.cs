using Dawem.Contract.Repository.Employees;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Generic;

namespace Dawem.Repository.Employees
{
    public class FingerprintEnforcementGroupRepository : GenericRepository<FingerprintEnforcementGroup>, IFingerprintEnforcementGroupRepository
    {
        public FingerprintEnforcementGroupRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
