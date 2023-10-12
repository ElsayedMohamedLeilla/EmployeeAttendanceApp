using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Generic;

namespace Dawem.Repository.Core
{
    public class UserGroupRepository : GenericRepository<UserGroup>, IUserGroupRepository
    {

        public UserGroupRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting generalSetting) : base(unitOfWork, generalSetting)
        {

        }

    }
}
