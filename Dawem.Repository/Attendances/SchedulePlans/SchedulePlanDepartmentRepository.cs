using Dawem.Contract.Repository.Attendances.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Generic;

namespace Dawem.Repository.Attendances.Schedules
{
    public class SchedulePlanDepartmentRepository : GenericRepository<SchedulePlanDepartment>, ISchedulePlanDepartmentRepository
    {
        public SchedulePlanDepartmentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
