using Dawem.Contract.Repository.Subscriptions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Providers
{
    public class PlanNameTranslationRepository : GenericRepository<PlanNameTranslation>, IPlanNameTranslationRepository
    {
        private readonly RequestInfo _requestInfo;
        public PlanNameTranslationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
    }
}
