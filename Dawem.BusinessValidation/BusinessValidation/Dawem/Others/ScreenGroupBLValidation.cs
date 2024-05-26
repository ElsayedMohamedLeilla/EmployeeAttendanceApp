using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;


namespace Dawem.Validation.BusinessValidation.AdminPanel.Subscriptions
{

    public class ScreenGroupBLValidation : IScreenGroupBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        private readonly INameTranslationBLValidationCore nameTranslationBLValidationCore;
        public ScreenGroupBLValidation(IRepositoryManager _repositoryManager,
            RequestInfo _requestInfo,
            INameTranslationBLValidationCore _nameTranslationBLValidationCore)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
            nameTranslationBLValidationCore = _nameTranslationBLValidationCore;
        }
        public async Task<bool> CreateValidation(CreateScreenGroupModel model)
        {
            #region Validate Arabic And English Languages

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #endregion

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateScreenGroupModel model)
        {
            #region Validate Arabic And English Languages

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #endregion

            return true;
        }
    }
}
