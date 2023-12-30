using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;

namespace Dawem.Repository.Others
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }

}
