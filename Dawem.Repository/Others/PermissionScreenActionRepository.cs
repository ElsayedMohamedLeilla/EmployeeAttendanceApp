
using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;

namespace Dawem.Repository.Others
{
    public class PermissionScreenActionRepository : GenericRepository<PermissionScreenAction>, IPermissionScreenActionRepository
    {
        public PermissionScreenActionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }

}
