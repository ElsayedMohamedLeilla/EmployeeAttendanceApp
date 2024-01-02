using Dawem.Contract.Repository.Permissions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;

namespace Dawem.Repository.Others
{
    public class PermissionScreenActionRepository : GenericRepository<PermissionScreenAction>, IPermissionScreenActionRepository
    {
        public PermissionScreenActionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }

}
