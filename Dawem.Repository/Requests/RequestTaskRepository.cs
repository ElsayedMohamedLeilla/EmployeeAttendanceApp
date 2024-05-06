using Azure.Core;
using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.Requests.Tasks;
using LinqKit;

namespace Dawem.Repository.Requests
{
    public class RequestTaskRepository : GenericRepository<RequestTask>, IRequestTaskRepository
    {
        private readonly RequestInfo requestInfo;
        public RequestTaskRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<RequestTask> GetAsQueryable(GetRequestTasksCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestTask>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestTask>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.TaskType != null && x.TaskType.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.And(x => x.TaskEmployees != null && x.TaskEmployees.Any(te => te.Employee.Name.ToLower().Trim().Contains(criteria.FreeText)));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestTask => !requestTask.Request.IsDeleted);
            predicate = predicate.And(requestTask => requestTask.Request.CompanyId == requestInfo.CompanyId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestTask => criteria.Ids.Contains(requestTask.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.Code == criteria.Code);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.EmployeeId == criteria.EmployeeId ||
                (requestTask.TaskEmployees != null && requestTask.TaskEmployees.Any(te => te.EmployeeId == criteria.EmployeeId)));
            }
            if (criteria.TaskTypeId is not null)
            {
                predicate = predicate.And(requestTask => requestTask.TaskTypeId == criteria.TaskTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestTask => criteria.Date.Value.Date >= requestTask.Request.Date.Date && criteria.Date.Value.Date <= requestTask.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<RequestTask> EmployeeGetAsQueryable(Employee2GetRequestTasksCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestTask>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestTask>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.TaskType != null && x.TaskType.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.And(x => x.TaskEmployees != null && x.TaskEmployees.Any(te => te.Employee.Name.ToLower().Trim().Contains(criteria.FreeText)));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestTask => !requestTask.Request.IsDeleted);
            predicate = predicate.And(requestTask => requestTask.Request.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(requestTask => requestTask.Request.EmployeeId == requestInfo.EmployeeId || 
            (requestTask.TaskEmployees != null && requestTask.TaskEmployees.Any(te => te.EmployeeId == requestInfo.EmployeeId)));
           
            if (criteria.Id != null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestTask => criteria.Ids.Contains(requestTask.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.Code == criteria.Code);
            }
            if (criteria.TaskTypeId is not null)
            {
                predicate = predicate.And(requestTask => requestTask.TaskTypeId == criteria.TaskTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestTask => requestTask.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestTask => criteria.Date.Value.Date >= requestTask.Request.Date.Date && criteria.Date.Value.Date <= requestTask.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
