﻿using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.Requests.Justifications;
using LinqKit;

namespace Dawem.Repository.Requests
{
    public class RequestJustificationRepository : GenericRepository<RequestJustification>, IRequestJustificationRepository
    {
        private readonly RequestInfo requestInfo;
        public RequestJustificationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork,
            GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<RequestJustification> GetAsQueryable(GetRequestJustificationCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestJustification>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestJustification>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.JustificatioType != null && x.JustificatioType.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestJustification => !requestJustification.Request.IsDeleted);
            predicate = predicate.And(requestJustification => requestJustification.Request.CompanyId == requestInfo.CompanyId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestJustification => criteria.Ids.Contains(requestJustification.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.Code == criteria.Code);
            }
            if (criteria.EmployeeId is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.JustificationTypeId is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.JustificationTypeId == criteria.JustificationTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestJustification => criteria.Date.Value.Date >= requestJustification.Request.Date.Date && criteria.Date.Value.Date <= requestJustification.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
        public IQueryable<RequestJustification> EmployeeGetAsQueryable(EmployeeGetRequestJustificationCriteria criteria)
        {
            var predicate = PredicateBuilder.New<RequestJustification>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<RequestJustification>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Request.Employee != null && x.Request.Employee.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.JustificatioType != null && x.JustificatioType.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int code))
                {
                    criteria.Code = code;
                }
            }

            predicate = predicate.And(requestJustification => !requestJustification.Request.IsDeleted);

            predicate = predicate.And(requestJustification => requestJustification.Request.CompanyId == requestInfo.CompanyId);

            predicate = predicate.And(requestJustification => requestJustification.Request.EmployeeId == requestInfo.EmployeeId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(requestJustification => criteria.Ids.Contains(requestJustification.Request.Id));
            }
            if (criteria.IsActive is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.IsActive == criteria.IsActive);
            }
            if (criteria.Code is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.Code == criteria.Code);
            }
            if (criteria.JustificationTypeId is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.JustificationTypeId == criteria.JustificationTypeId);
            }
            if (criteria.Status is not null)
            {
                predicate = predicate.And(requestJustification => requestJustification.Request.Status == criteria.Status);
            }
            if (criteria.Date is not null)
            {
                predicate = predicate.And(requestJustification => criteria.Date.Value.Date >= requestJustification.Request.Date.Date && criteria.Date.Value.Date <= requestJustification.DateTo.Date);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }
}
