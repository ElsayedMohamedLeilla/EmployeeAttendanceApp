using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Requests.Task;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Employees
{
    public class RequestTaskRepository : GenericRepository<RequestTask>, IRequestTaskRepository
    {
        public RequestTaskRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<RequestTask> GetAsQueryable(GetRequestTasksCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestTask>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestTask>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
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
                predicate = predicate.And(e => e.Request.Code == criteria.Code);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(e => e.Request.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.TaskTypeId is not null)
            {
                predicate = predicate.And(e => e.TaskTypeId == criteria.TaskTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(e => e.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(e => criteria.Date.Value.Date >= e.Request.Date.Date && criteria.Date.Value.Date <= e.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
