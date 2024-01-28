using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Generic;

namespace Dawem.Repository.Summons
{
    public class SummonDepartmentRepository : GenericRepository<SummonDepartment>, ISummonDepartmentRepository
    {
        public SummonDepartmentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
