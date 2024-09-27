using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Core.DefaultLookups
{
    public class DefaultLookupsNameTranslationRepository : GenericRepository<DefaultLookupsNameTranslation>, IDefaultLookupsNameTranslationRepository
    {
        private readonly RequestInfo _requestInfo;
        public DefaultLookupsNameTranslationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
    }
}
