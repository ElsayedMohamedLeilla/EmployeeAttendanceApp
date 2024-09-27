using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Providers;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class CompanyBranchRepository : GenericRepository<CompanyBranch>, ICompanyBranchRepository
    {

        private readonly RequestInfo requestInfo;
        public CompanyBranchRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _requestInfo
            , GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

            requestInfo = _requestInfo;
        }

        public IQueryable<CompanyBranch> GetAsQueryable(GetCompanyBranchesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<CompanyBranch>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<CompanyBranch>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.Start(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Address.ToLower().Trim().StartsWith(criteria.FreeText));

                int id;
                if (int.TryParse(criteria.FreeText, out id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.Id != null)
            {
                predicate = predicate.And(x => x.Id == criteria.Id.Value);
                predicate = predicate.And(inner);
                var Query = Get(predicate);
                return Query;
            }
            else
            {
                predicate = predicate.And(x => x.CompanyId == requestInfo.CompanyId);
                predicate = predicate.And(inner);
                var Query = Get(predicate);
                return Query;
            }
        }
    }
}
