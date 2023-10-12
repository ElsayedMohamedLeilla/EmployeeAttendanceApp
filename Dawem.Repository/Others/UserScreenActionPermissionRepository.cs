using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Ohters;
using Dawem.Models.Context;
using Dawem.Repository.Others.Conract;

namespace Dawem.Repository.Others
{
    public class UserScreenActionPermissionRepository : GenericRepository<UserScreenActionPermission>, IUserScreenActionPermissionRepository
    {
        private readonly RequestHeaderContext userContext;
        public UserScreenActionPermissionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestHeaderContext _userContext) : base(unitOfWork)
        {
            userContext = _userContext;
        }
    }

}
