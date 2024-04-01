using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Summons
{
    public class SummonActionRepository : GenericRepository<SummonSanction>, ISummonSanctionRepository
    {
        public SummonActionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
