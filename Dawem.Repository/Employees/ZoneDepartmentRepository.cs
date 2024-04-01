using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Employees
{
    public class ZoneDepartmentRepository : GenericRepository<ZoneDepartment>, IZoneDepartmentRepository
    {
        public ZoneDepartmentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
