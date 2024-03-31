using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Core.Groups
{
    public class GroupManagerDelegatorRepository : GenericRepository<GroupManagerDelegator>, IGroupManagerDelegatorRepository
    {
        public GroupManagerDelegatorRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }



    }
}
