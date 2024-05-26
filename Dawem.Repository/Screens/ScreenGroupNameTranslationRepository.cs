using Dawem.Contract.Repository.Settings;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Providers
{
    public class ScreenGroupNameTranslationRepository : GenericRepository<ScreenGroupNameTranslation>, IScreenGroupNameTranslationRepository
    {
        private readonly RequestInfo _requestInfo;
        public ScreenGroupNameTranslationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
    }
}
