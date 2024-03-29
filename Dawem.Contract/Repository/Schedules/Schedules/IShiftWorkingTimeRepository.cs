using Dawem.Data;
using Dawem.Domain.Entities.Schedules;
using Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes;

namespace Dawem.Contract.Repository.Schedules.Schedules
{
    public interface IShiftWorkingTimeRepository : IGenericRepository<ShiftWorkingTime>
    {
        IQueryable<ShiftWorkingTime> GetAsQueryable(GetShiftWorkingTimesCriteria criteria);
    }
}
