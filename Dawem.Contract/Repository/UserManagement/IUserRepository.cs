using Dawem.Data;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Translations;

namespace Dawem.Contract.Repository.UserManagement
{
    public interface IUserRepository : IGenericRepository<MyUser>
    {
        IQueryable<MyUser> GetAsQueryable(UserSearchCriteria criteria, string includeProperties = DawemKeys.EmptyString);
    }
}
