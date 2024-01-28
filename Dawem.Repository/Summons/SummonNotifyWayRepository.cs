using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Generic;

namespace Dawem.Repository.Summons
{
    public class SummonNotifyWayRepository : GenericRepository<SummonNotifyWay>, ISummonNotifyWayRepository
    {
        public SummonNotifyWayRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
