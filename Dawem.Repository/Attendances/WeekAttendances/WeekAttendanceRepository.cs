using Dawem.Contract.Repository.Attendances.WeekAttendances;
using Dawem.Contract.Repository.Employees;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Attendances.WeekAttendances
{
    public class WeekAttendanceRepository : GenericRepository<WeekAttendance>, IWeekAttendanceRepository
    {
        public WeekAttendanceRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<WeekAttendance> GetAsQueryable(GetWeekAttendancesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<WeekAttendance>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<WeekAttendance>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
