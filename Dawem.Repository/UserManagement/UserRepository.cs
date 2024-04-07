using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Translations;
using LinqKit;

namespace Dawem.Repository.UserManagement
{
    public class UserRepository : GenericRepository<MyUser>, IUserRepository
    {
        private readonly RequestInfo requestInfo;
        public UserRepository(RequestInfo _requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }

        public IQueryable<MyUser> GetAsQueryableOld(UserSearchCriteria criteria, string includeProperties = LeillaKeys.EmptyString)
        {
            var userPredicate = PredicateBuilder.New<MyUser>(true);

            userPredicate = userPredicate.And(x => x.UserBranches.Any(a => a.BranchId == requestInfo.BranchId));

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
            if (criteria.Code != null)
            {
                userPredicate = userPredicate.And(ps => ps.Code == criteria.Code);
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

            if (requestInfo.Type == AuthenticationType.AdminPanel)
            {
                predicate = predicate.And(e => e.CompanyId == null);
            }
            else if (requestInfo.Type == AuthenticationType.DawemAdmin)
            {
                predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);
            }

            predicate = predicate.And(e => e.Type == requestInfo.Type);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));
                inner = inner.And(x => x.Email != null && x.Email.ToLower().Trim().Contains(criteria.FreeText));
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
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }
    }

}
