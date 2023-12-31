using Dawem.Contract.Repository.Permissions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;

namespace Dawem.Repository.Others
{
    public class PermissionScreenRepository : GenericRepository<PermissionScreen>, IPermissionScreenRepository
    {
        public PermissionScreenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }

}
