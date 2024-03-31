using Dawem.Contract.Repository.Localization;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Localization;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Localizations
{
    public class TranslationRepository : GenericRepository<Translation>, ITranslationRepository
    {
        public TranslationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
