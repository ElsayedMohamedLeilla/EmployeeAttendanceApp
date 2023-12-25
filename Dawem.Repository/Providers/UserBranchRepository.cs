using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Generic;

namespace Dawem.Repository.Providers
{
    public class UserBranchRepository : GenericRepository<UserBranch>, IUserBranchRepository
    {
        public UserBranchRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
