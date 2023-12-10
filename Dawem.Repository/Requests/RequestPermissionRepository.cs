using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Requests.Permissions;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Requests
{
    public class RequestPermissionRepository : GenericRepository<RequestPermission>, IRequestPermissionRepository
    {
        private readonly RequestInfo requestInfo;
        public RequestPermissionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<RequestPermission> GetAsQueryable(GetRequestPermissionsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestPermission>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestPermission>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestPermission => requestPermission.Request.CompanyId == requestInfo.CompanyId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestPermission => criteria.Ids.Contains(requestPermission.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.Code == criteria.Code);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.PermissionTypeId is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.PermissionTypeId == criteria.PermissionTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestPermission => criteria.Date.Value.Date >= requestPermission.Request.Date.Date && criteria.Date.Value.Date <= requestPermission.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<RequestPermission> EmployeeGetAsQueryable(EmployeeGetRequestPermissionsCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestPermission>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestPermission>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestPermission => requestPermission.Request.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(requestPermission => requestPermission.Request.EmployeeId == requestInfo.EmployeeId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestPermission => criteria.Ids.Contains(requestPermission.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.Code == criteria.Code);
            }
            if (criteria.PermissionTypeId is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.PermissionTypeId == criteria.PermissionTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestPermission => requestPermission.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestPermission => criteria.Date.Value.Date >= requestPermission.Request.Date.Date && criteria.Date.Value.Date <= requestPermission.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
