using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Providers;
using Dawem.Models.Dtos.Identities;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Providers
{
    public class BranchRepository : GenericRepository<CompanyBranch>, IBranchRepository
    {

        private readonly RequestInfo requestHeaderContext;
        public BranchRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _requestHeaderContext
            , GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

            requestHeaderContext = _requestHeaderContext;
        }

        public IQueryable<CompanyBranch> GetAsQueryable(GetBranchesCriteria criteria, string includeProperties = "Company", UserDTO user = null)
        {


            var predicate = PredicateBuilder.New<CompanyBranch>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<CompanyBranch>(true);


            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));

                inner = inner.Or(x => x.Address.ToLower().Trim().Contains(criteria.FreeText));


                int id;
                if (int.TryParse(criteria.FreeText, out id))
                {
                    criteria.Id = id;
                }
            }


            if (criteria.GetMainBranch != null)
            {
                if (criteria.GetMainBranch.Value)
                {
                    predicate = predicate.And(x => x.IsMainBranch);
                }
                else
                {

                    predicate = predicate.And(x => !x.IsMainBranch);
                }

            }

            if (criteria.Id != null)
            {
                predicate = predicate.And(x => x.Id == criteria.Id.Value);
                predicate = predicate.And(inner);
                var Query = Get(predicate, includeProperties: includeProperties);
                return Query;

            }

            else
            {

                if (criteria.BranchName != null)
                {
                    criteria.BranchName = criteria.BranchName.ToLower().TrimStart().TrimEnd();
                    predicate = predicate.And(x => x.Name.Contains(criteria.BranchName));
                }

                if (criteria.UserId != null)
                {
                    predicate = predicate.And(c => c.UserBranches.Any(ua => ua.UserId == criteria.UserId));
                }

                if (criteria.CompanyId != null && criteria.CompanyId != default(int))
                {
                    predicate = predicate.And(x => x.CompanyId == criteria.CompanyId);
                }
                else
                {
                    predicate = predicate.And(x => x.CompanyId == requestHeaderContext.CompanyId);
                }

                predicate = predicate.And(inner);
                var Query = Get(predicate, includeProperties: includeProperties);

                return Query;


            }

        }

    }
}
