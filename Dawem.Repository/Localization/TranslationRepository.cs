
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Localization;
using Dawem.Models.Generic;
using Dawem.Repository.Localization.Contract;

namespace Dawem.Repository.Localization
{
    public class TranslationRepository : GenericRepository<Translation>, ITranslationRepository
    {
        public TranslationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
