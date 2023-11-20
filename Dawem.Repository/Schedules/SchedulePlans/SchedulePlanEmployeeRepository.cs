using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Generic;

namespace Dawem.Repository.Schedules.SchedulePlans
{
    public class SchedulePlanEmployeeRepository : GenericRepository<SchedulePlanEmployee>, ISchedulePlanEmployeeRepository
    {
        public SchedulePlanEmployeeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
