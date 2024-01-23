using Dawem.Contract.Repository.Employees;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Employees
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly RequestInfo requestInfo;
        public EmployeeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<Employee> GetAsQueryable(GetEmployeesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Employee>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Employee>(true);

            predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);

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
            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
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
            if (criteria.EmployeeNumber is not null)
            {
                predicate = predicate.And(e => e.EmployeeNumber == criteria.EmployeeNumber);
            }
            if (criteria.Status != null)
            {
                var clientLocalDate = requestInfo.LocalDateTime;

                switch (criteria.Status.Value)
                {
                    case FilterEmployeeStatus.Available:
                        predicate = predicate.And(employee => !employee.EmployeeTasks.Any(task => !task.IsDeleted && !task.RequestTask.Request.IsDeleted 
                        && (task.RequestTask.Request.Status == RequestStatus.Accepted || task.RequestTask.Request.Status == RequestStatus.Pending) 
                        && clientLocalDate.Date >= task.RequestTask.Request.Date && clientLocalDate.Date <= task.RequestTask.DateTo)
                        &&
                        !employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestTask.Request.IsDeleted
                        && (request.RequestTask.Request.Status == RequestStatus.Accepted || request.RequestTask.Request.Status == RequestStatus.Pending)
                        && (request.RequestTask.Request.Type == RequestType.Assignment || request.RequestTask.Request.Type == RequestType.Vacation)
                        && clientLocalDate.Date >= request.Date.Date
                        && clientLocalDate.Date <= request.Date.Date));

                        break;
                    case FilterEmployeeStatus.InTaskOrAssignment:
                        predicate = predicate.And(employee => employee.EmployeeTasks.Any(task => !task.IsDeleted && !task.RequestTask.Request.IsDeleted
                        && (task.RequestTask.Request.Status == RequestStatus.Accepted || task.RequestTask.Request.Status == RequestStatus.Pending)
                        && clientLocalDate.Date >= task.RequestTask.Request.Date
                        && clientLocalDate.Date <= task.RequestTask.DateTo)
                        &&
                        !employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestTask.Request.IsDeleted
                        && (request.RequestTask.Request.Status == RequestStatus.Accepted || request.RequestTask.Request.Status == RequestStatus.Pending)
                        && request.RequestTask.Request.Type == RequestType.Assignment
                        && clientLocalDate.Date >= request.Date.Date
                        && clientLocalDate.Date <= request.Date.Date));

                        break;
                    case FilterEmployeeStatus.InVacationOrOutside:
                        predicate = predicate.And(employee =>
                        employee.EmployeeRequests.Any(request => !request.IsDeleted && !request.RequestTask.Request.IsDeleted
                        && (request.RequestTask.Request.Status == RequestStatus.Accepted || request.RequestTask.Request.Status == RequestStatus.Pending)
                        && request.RequestTask.Request.Type == RequestType.Vacation
                        && clientLocalDate.Date >= request.Date.Date
                        && clientLocalDate.Date <= request.Date.Date));

                        break;
                    default:
                        break;
                }
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
