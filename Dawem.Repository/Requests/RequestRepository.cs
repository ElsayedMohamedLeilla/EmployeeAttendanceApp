using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Employees
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        public RequestRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<Request> GetAsQueryable(GetRequestsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Request>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Request>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Employee != null && x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
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
            if (criteria.Type is not null)
            {
                predicate = predicate.And(e => e.Type == criteria.Type);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(e => e.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(e => e.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(e => e.Date == criteria.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
