
using Dawem.Contract.Repository.Summons;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Context;
using Dawem.Models.Generic;

namespace Dawem.Repository.Summons
{
    public class SummonLogSanctionRepository : GenericRepository<SummonLogSanction>, ISummonLogSanctionRepository
    {
        private readonly RequestInfo _requestInfo;
        public SummonLogSanctionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
    }
}
