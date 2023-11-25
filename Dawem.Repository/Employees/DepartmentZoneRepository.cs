using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Generic;

namespace Dawem.Repository.Employees
{
    public class DepartmentZoneRepository : GenericRepository<DepartmentZone>, IDepartmentZoneRepository
    {
        public DepartmentZoneRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
