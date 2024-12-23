using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.Requests.Justifications;
using LinqKit;

namespace Dawem.Repository.Requests
{
    public class RequestOvertimeRepository : GenericRepository<RequestOvertime>, IRequestOvertimeRepository
    {
        private readonly RequestInfo requestInfo;
        public RequestOvertimeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<RequestOvertime> GetAsQueryable(GetRequestOvertimeCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestOvertime>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestOvertime>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.OvertimeType != null && x.OvertimeType.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestVacation => !requestVacation.Request.IsDeleted);
            predicate = predicate.And(requestVacation => requestVacation.Request.CompanyId == requestInfo.CompanyId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestVacation => criteria.Ids.Contains(requestVacation.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Code == criteria.Code);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.OvertimeTypeId is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.OvertimeTypeId == criteria.OvertimeTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestVacation => criteria.Date.Value.Date >= requestVacation.Request.Date.Date && criteria.Date.Value.Date <= requestVacation.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<RequestOvertime> EmployeeGetAsQueryable(EmployeeGetRequestOvertimeCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestOvertime>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestOvertime>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.OvertimeType != null && x.OvertimeType.Name.ToLower().Trim().StartsWith(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestVacation => !requestVacation.Request.IsDeleted);

            predicate = predicate.And(requestVacation => requestVacation.Request.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(requestVacation => requestVacation.Request.EmployeeId == requestInfo.EmployeeId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestVacation => criteria.Ids.Contains(requestVacation.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Code == criteria.Code);
            }
            if (criteria.OvertimeTypeId is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.OvertimeTypeId == criteria.OvertimeTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestVacation => requestVacation.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestVacation => criteria.Date.Value.Date >= requestVacation.Request.Date.Date && criteria.Date.Value.Date <= requestVacation.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
