using Dawem.Contract.Repository.Attendances.WeekAttendances;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Models.Generic;

namespace Dawem.Repository.Attendances.WeekAttendances
{
    public class WeekAttendanceShiftRepository : GenericRepository<WeekAttendanceShift>, IWeekAttendanceShiftRepository
    {
        public WeekAttendanceShiftRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
