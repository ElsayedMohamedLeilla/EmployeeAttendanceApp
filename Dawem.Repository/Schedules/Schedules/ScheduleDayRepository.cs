using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Generic;

namespace Dawem.Repository.Schedules.Schedules
{
    public class ScheduleDayRepository : GenericRepository<ScheduleDay>, IScheduleDayRepository
    {
        public ScheduleDayRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
