using Dawem.Contract.Repository.Attendances.Schedules;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Generic;

namespace Dawem.Repository.Attendances.Schedules
{
    public class SchedulePlanGroupRepository : GenericRepository<SchedulePlanGroup>, ISchedulePlanGroupRepository
    {
        public SchedulePlanGroupRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
