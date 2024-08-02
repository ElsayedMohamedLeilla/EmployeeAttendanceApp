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
using System.Linq;

namespace Dawem.Repository.UserManagement
{
    public class UserRepository : GenericRepository<MyUser>, IUserRepository
    {
        private readonly RequestInfo requestInfo;
        private ApplicationDBContext Context { get; set; }

        public UserRepository(RequestInfo _requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
            Context = unitOfWork.Context;
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

                userPredicate = userPredicate.Start(x => x.UserName.ToLower().Trim().StartsWith(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.Email.ToLower().Trim().StartsWith(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.MobileNumber.ToLower().Trim().StartsWith(criteria.FreeText));
                userPredicate = userPredicate.Or(x => x.PhoneNumber.ToLower().Trim().StartsWith(criteria.FreeText));
            }
            if (criteria.Code != null)
            {
                userPredicate = userPredicate.And(ps => ps.Code == criteria.Code);
            }

            if (!string.IsNullOrWhiteSpace(criteria.UserName))
            {
                userPredicate = userPredicate.And(x => x.UserName.ToLower().Trim().StartsWith(criteria.UserName.ToLower().Trim()));
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

            if (requestInfo.AuthenticationType == AuthenticationType.AdminPanel)
            {
                predicate = predicate.And(e => e.CompanyId == null);
            }
            else if (requestInfo.AuthenticationType == AuthenticationType.DawemAdmin)
            {
                predicate = predicate.And(e => e.CompanyId == requestInfo.CompanyId);
            }
            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            predicate = predicate.And(e => e.Type == requestInfo.AuthenticationType);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Email != null && x.Email.ToLower().Trim().StartsWith(criteria.FreeText));
                inner = inner.Or(x => x.Employee != null && x.Employee.Name.ToLower().Trim().StartsWith(criteria.FreeText));

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
        public List<int?> GetEmployeeIdsNotConnectedToUser()
        {
            var employeeIdsWithUsers = Context.MyUser.Select(u => u.EmployeeId).ToList();
            var allEmployeeIds = Context.Employees.Select(e => e.Id).ToList();
            var result = allEmployeeIds.Where(id => !employeeIdsWithUsers.Contains(id)).Select(id => (int?)id).ToList();
            return (List<int?>)result;
        }
    }

}
