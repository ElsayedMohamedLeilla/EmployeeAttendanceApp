using Dawem.Contract.Repository.Attendances.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Generic;

namespace Dawem.Repository.Attendances.Schedules
{
    public class SchedulePlanBackgroundJobLogEmployeeRepository : GenericRepository<SchedulePlanBackgroundJobLogEmployee>, ISchedulePlanBackgroundJobLogEmployeeRepository
    {
        public SchedulePlanBackgroundJobLogEmployeeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
