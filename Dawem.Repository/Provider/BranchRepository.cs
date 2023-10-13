using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Provider;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Identity;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Provider
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {

        private readonly RequestHeaderContext requestHeaderContext;
        public BranchRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestHeaderContext _requestHeaderContext
            , GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

            requestHeaderContext = _requestHeaderContext;
        }

        public IQueryable<Branch> GetAsQueryable(GetBranchesCriteria criteria, string includeProperties = "Company", UserDTO user = null)
        {


            var predicate = PredicateBuilder.New<Branch>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Branch>(true);


            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.BranchName.ToLower().Trim().Contains(criteria.FreeText));

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
                    predicate = predicate.And(x => x.BranchName.Contains(criteria.BranchName));
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
