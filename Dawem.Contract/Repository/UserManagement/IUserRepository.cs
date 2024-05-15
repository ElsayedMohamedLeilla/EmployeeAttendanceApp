using Dawem.Data;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Criteria.UserManagement;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using DocumentFormat.OpenXml.InkML;

namespace Dawem.Contract.Repository.UserManagement
{
    public interface IUserRepository : IGenericRepository<MyUser>
    {
        IQueryable<MyUser> GetAsQueryableOld(UserSearchCriteria criteria, string includeProperties = LeillaKeys.EmptyString);
        IQueryable<MyUser> GetAsQueryable(GetUsersCriteria criteria);
        public List<int?> GetEmployeeIdsNotConnectedToUser();
        
    }
}
