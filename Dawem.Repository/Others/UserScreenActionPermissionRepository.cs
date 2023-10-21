using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Ohters;
using Dawem.Models.Context;

namespace Dawem.Repository.Others
{
    public class UserScreenActionPermissionRepository : GenericRepository<UserScreenActionPermission>, IUserScreenActionPermissionRepository
    {
        private readonly RequestInfo userContext;
        public UserScreenActionPermissionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _userContext) : base(unitOfWork)
        {
            userContext = _userContext;
        }
    }

}
