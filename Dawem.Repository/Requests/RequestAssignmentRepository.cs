using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
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

                inner = inner.Start(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                //inner = inner.Or(x => x.AssignmentType != null && x.AssignmentType.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestVacation => !requestVacation.Request.IsDeleted);
            predicate = predicate.And(requestVacation => requestVacation.Request.CompanyId == requestInfo.CompanyId);

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
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(requestAssignment => requestAssignment.Request.EmployeeId == criteria.EmployeeId);
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
        public IQueryable<RequestAssignment> EmployeeGetAsQueryable(Employee2GetRequestAssignmentsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestAssignment>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestAssignment>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.AssignmentType != null && x.AssignmentType.Name.ToLower().Trim().StartsWith(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestAssignment => !requestAssignment.Request.IsDeleted);

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
