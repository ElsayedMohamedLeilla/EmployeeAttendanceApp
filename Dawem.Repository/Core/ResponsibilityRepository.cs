﻿using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Core
{
    public class ResponsibilityRepository : GenericRepository<Responsibility>, IResponsibilityRepository
    {
        private readonly RequestInfo requestInfo;
        public ResponsibilityRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }

        public IQueryable<Responsibility> GetAsQueryable(GetResponsibilitiesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Responsibility>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Responsibility>(true);

            if (requestInfo.AuthenticationType == AuthenticationType.AdminPanel)
            {
                predicate = predicate.And(e => e.CompanyId == null);
            }
            else if (requestInfo.AuthenticationType == AuthenticationType.DawemAdmin)
            {
                predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);
            }

            predicate = predicate.And(e => e.Type == requestInfo.AuthenticationType);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.Start(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.Id != null)
            {

                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
