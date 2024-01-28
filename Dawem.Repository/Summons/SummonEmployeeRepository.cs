using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Generic;

namespace Dawem.Repository.Summons
{
    public class SummonEmployeeRepository : GenericRepository<SummonEmployee>, ISummonEmployeeRepository
    {
        public SummonEmployeeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
