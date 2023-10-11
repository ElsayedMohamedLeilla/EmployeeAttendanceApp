using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Provider;
using Dawem.Models.Generic;
using Dawem.Repository.Provider.Contract;

namespace Dawem.Repository.Provider
{
    public class UserBranchRepository : GenericRepository<UserBranch>, IUserBranchRepository
    {
        public UserBranchRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
