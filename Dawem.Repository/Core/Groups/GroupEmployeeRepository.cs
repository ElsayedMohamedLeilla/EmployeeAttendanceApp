using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Core.Groups
{
    public class GroupEmployeeRepository : GenericRepository<GroupEmployee>, IGroupEmployeeRepository
    {
        public GroupEmployeeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }



    }
}
