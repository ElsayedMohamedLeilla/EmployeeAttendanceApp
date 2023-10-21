using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Context;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Generic;
using Dawem.Translations;
using LinqKit;

namespace Dawem.Repository.UserManagement
{
    public class UserRepository : GenericRepository<MyUser>, IUserRepository
    {
        private readonly RequestInfo requestHeaderContext;
        public UserRepository(RequestInfo _requestHeaderContext, IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
            requestHeaderContext = _requestHeaderContext;
        }

        public IQueryable<MyUser> GetAsQueryable(UserSearchCriteria criteria, string includeProperties = DawemKeys.EmptyString)
        {
            var userPredicate = PredicateBuilder.New<MyUser>(true);

            if (requestHeaderContext.IsMainBranch && criteria.ForGridView)
            {

                userPredicate = userPredicate.And(x => x.BranchId == requestHeaderContext.BranchId);
            }
            else
            {
                userPredicate = userPredicate.And(x => x.UserBranches.Any(a => a.BranchId == requestHeaderContext.BranchId));
            }

            if (criteria.Id is not null)
            {
                userPredicate = userPredicate.And(x => x.Id == criteria.Id);
            }

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                userPredicate = userPredicate.Start(x => x.UserName.ToLower().Trim().Contains(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.FirstName.ToLower().Trim().Contains(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.LastName.ToLower().Trim().Contains(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.Email.ToLower().Trim().Contains(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.MobileNumber.ToLower().Trim().Contains(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.PhoneNumber.ToLower().Trim().Contains(criteria.FreeText));
            }

            if (!string.IsNullOrWhiteSpace(criteria.UserName))
            {
                userPredicate = userPredicate.And(x => x.UserName.ToLower().Trim().Contains(criteria.UserName.ToLower().Trim()));
            }

            if (criteria.IsActive != null)
            {
                userPredicate = userPredicate.And(x => x.IsActive == true);
            }

            var query = Get(userPredicate, includeProperties: includeProperties);
            return query;
        }
    }

}
