using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Generic;

namespace Dawem.Repository.Employees
{
    public class DepartmentManagerDelegatorRepository : GenericRepository<DepartmentManagerDelegator>, IDepartmentManagerDelegatorRepository
    {
        public DepartmentManagerDelegatorRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }



    }
}
