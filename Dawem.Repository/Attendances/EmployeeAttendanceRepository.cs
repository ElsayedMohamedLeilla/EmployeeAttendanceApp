using Dawem.Contract.Repository.Employees;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.HolidayType;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Employees
{
    public class EmployeeAttendanceRepository : GenericRepository<EmployeeAttendance>, IEmployeeAttendanceRepository
    {
        private readonly RequestInfo requestInfo;
        public EmployeeAttendanceRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<EmployeeAttendance> GetAsQueryable(GetEmployeeAttendancesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<EmployeeAttendance>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<EmployeeAttendance>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(inner);

            var Query = Get(predicate);
            return Query;

        }
    }
}
