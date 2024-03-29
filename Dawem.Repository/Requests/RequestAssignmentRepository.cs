using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Models.Requests.Assignments;
using LinqKit;

namespace Dawem.Repository.Requests
{
    public class RequestAssignmentRepository : GenericRepository<RequestAssignment>, IRequestAssignmentRepository
    {
        private readonly RequestInfo requestInfo;
        public RequestAssignmentRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<RequestAssignment> GetAsQueryable(GetRequestAssignmentsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestAssignment>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestAssignment>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.AssignmentType != null && x.AssignmentType.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestVacation => requestVacation.Request.CompanyId == requestInfo.CompanyId);

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
                predicate = predicate.And(requestTask => requestTask.Request.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.AssignmentTypeId is not null)
            {
                predicate = predicate.And(requestTask => requestTask.AssignmentTypeId == criteria.AssignmentTypeId);
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
        public IQueryable<RequestAssignment> EmployeeGetAsQueryable(Employee2GetRequestAssignmentsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestAssignment>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestAssignment>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.AssignmentType != null && x.AssignmentType.Name.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestAssignment => requestAssignment.Request.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(requestAssignment => requestAssignment.Request.EmployeeId == requestInfo.EmployeeId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestAssignment => requestAssignment.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestAssignment => criteria.Ids.Contains(requestAssignment.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestAssignment => requestAssignment.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestAssignment => requestAssignment.Request.Code == criteria.Code);
            }
            if (criteria.AssignmentTypeId is not null)
            {
                predicate = predicate.And(requestAssignment => requestAssignment.AssignmentTypeId == criteria.AssignmentTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestAssignment => requestAssignment.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestAssignment => criteria.Date.Value.Date >= requestAssignment.Request.Date.Date && criteria.Date.Value.Date <= requestAssignment.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
