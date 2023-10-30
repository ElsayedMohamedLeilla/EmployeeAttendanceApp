using Dawem.Data;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Employees.User;
using Dawem.Translations;

namespace Dawem.Contract.Repository.UserManagement
{
    public interface IUserRepository : IGenericRepository<MyUser>
    {
        IQueryable<MyUser> GetAsQueryableOld(UserSearchCriteria criteria, string includeProperties = DawemKeys.EmptyString);
        IQueryable<MyUser> GetAsQueryable(GetUsersCriteria criteria);
    }
}
