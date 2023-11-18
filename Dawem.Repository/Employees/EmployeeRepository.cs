using Dawem.Contract.Repository.Employees;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Employees
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<Employee> GetAsQueryable(GetEmployeesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Employee>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Employee>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.Department.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.JobTitle.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.Schedule.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.DirectManager.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.Email.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.MobileNumber.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.Address.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(e => e.Code == criteria.Code);
            }
            if (criteria.DepartmentId is not null)
            {
                predicate = predicate.And(e => e.DepartmentId == criteria.DepartmentId);
            }
            if (criteria.JobTitleId is not null)
            {
                predicate = predicate.And(e => e.JobTitleId == criteria.JobTitleId);
            }
            if (criteria.ScheduleId is not null)
            {
                predicate = predicate.And(e => e.ScheduleId == criteria.ScheduleId);
            }
            if (criteria.DirectManagerId is not null)
            {
                predicate = predicate.And(e => e.DirectManagerId == criteria.DirectManagerId);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
