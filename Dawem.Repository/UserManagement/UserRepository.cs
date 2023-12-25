using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Context;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Employees.Users;
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

        public IQueryable<MyUser> GetAsQueryableOld(UserSearchCriteria criteria, string includeProperties = LeillaKeys.EmptyString)
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
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                userPredicate = userPredicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                userPredicate = userPredicate.Start(x => x.UserName.ToLower().Trim().Contains(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));
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

        public IQueryable<MyUser> GetAsQueryable(GetUsersCriteria criteria)
        {
            var predicate = PredicateBuilder.New<MyUser>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<MyUser>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.Or(x => x.Employee != null && x.Employee.Name.ToLower().Trim().Contains(criteria.FreeText));

                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }

}
