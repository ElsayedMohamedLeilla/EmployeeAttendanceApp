using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;

namespace Dawem.Repository.Others
{
    public class PermissionScreenRepository : GenericRepository<PermissionScreen>, IPermissionScreenRepository
    {
        public PermissionScreenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }

}
