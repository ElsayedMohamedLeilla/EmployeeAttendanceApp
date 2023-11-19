using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Generic;

namespace Dawem.Repository.Core.GroupEmployees
{
    public class GroupManagerDelegatorRepository : GenericRepository<GroupManagerDelegator>, IGroupManagerDelegatorRepository
    {
        public GroupManagerDelegatorRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }



    }
}
