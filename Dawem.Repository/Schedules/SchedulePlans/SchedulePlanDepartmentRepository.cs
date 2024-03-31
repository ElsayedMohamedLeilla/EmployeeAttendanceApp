using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Schedules.SchedulePlans
{
    public class SchedulePlanDepartmentRepository : GenericRepository<SchedulePlanDepartment>, ISchedulePlanDepartmentRepository
    {
        public SchedulePlanDepartmentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
