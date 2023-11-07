using Dawem.Contract.Repository.Attendances.WeekAttendances;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Generic;

namespace Dawem.Repository.Attendances.WeekAttendances
{
    public class ScheduleDayRepository : GenericRepository<ScheduleDay>, IScheduleDayRepository
    {
        public ScheduleDayRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
