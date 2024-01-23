using Dawem.Contract.Repository.Attendances;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendances;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Dtos.Dashboard;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Attendances
{
    public class EmployeeAttendanceRepository : GenericRepository<EmployeeAttendance>, IEmployeeAttendanceRepository
    {
        private readonly RequestInfo requestInfo;
        public EmployeeAttendanceRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _attendanceInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _attendanceInfo;
        }
        public IQueryable<EmployeeAttendance> GetAsQueryable(GetEmployeeAttendancesForWebAdminCriteria criteria)
        {
            var predicate = PredicateBuilder.New<EmployeeAttendance>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<EmployeeAttendance>(true);

            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int employeeNumber))
                {
                    criteria.EmployeeNumber = employeeNumber;
                }
            }
            if (criteria.EmployeeNumber != null)
            {
                predicate = predicate.And(e => e.Employee.EmployeeNumber == criteria.EmployeeNumber);
            }
            if (criteria.EmployeeId != null)
            {
                predicate = predicate.And(e => e.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.Date != null)
            {
                predicate = predicate.And(e => e.LocalDate.Date == criteria.Date.Value.Date);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            

            predicate = predicate.And(inner);

            var Query = Get(predicate);
            return Query;
        }
        public IQueryable<EmployeeAttendance> GetForStatusAsQueryable(GetStatusBaseModel model)
        {
            var predicate = PredicateBuilder.New<EmployeeAttendance>(a => !a.IsDeleted && a.CompanyId == requestInfo.CompanyId);

            switch (model.Type)
            {
                case GetRequestsStatusType.CurrentDay:
                    predicate = predicate.And(attendance => attendance.LocalDate.Date == model.LocalDate.Date);
                    break;
                case GetRequestsStatusType.CurrentMonth:
                    predicate = predicate.And(attendance => attendance.LocalDate.Month == model.LocalDate.Month && attendance.LocalDate.Year == model.LocalDate.Year);
                    break;
                case GetRequestsStatusType.CurrentYear:
                    predicate = predicate.And(attendance => attendance.LocalDate.Year == model.LocalDate.Year);
                    break;
                default:
                    break;
            }

            if (model.DateFrom != null)
            {
                predicate = predicate.And(attendance => attendance.LocalDate >= model.DateFrom.Value);
            }
            if (model.DateTo != null)
            {
                predicate = predicate.And(attendance => attendance.LocalDate <= model.DateTo.Value);
            }
            var Query = Get(predicate);
            return Query;
        }
    }
}
