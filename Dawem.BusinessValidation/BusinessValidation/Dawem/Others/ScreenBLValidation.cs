using Dawem.Contract.BusinessValidation.Dawem.Others;
using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.Dawem.Others
{

    public class ScreenBLValidation : IScreenBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        private readonly INameTranslationBLValidationCore nameTranslationBLValidationCore;
        public ScreenBLValidation(IRepositoryManager _repositoryManager,
            RequestInfo _requestInfo,
            INameTranslationBLValidationCore _nameTranslationBLValidationCore)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
            nameTranslationBLValidationCore = _nameTranslationBLValidationCore;
        }
        public async Task<bool> CreateValidation(CreateScreenModel model)
        {
            #region Validate Name Translations

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #region Validate Duplication

            await ValidateScreenNameDuplication(model.NameTranslations);

            #endregion

            #endregion

            await ValidateOrderDuplication(model.Order, 0, model.AuthenticationType);

            await ValidateParent(model.ParentId);

            await CheckParentAuthenticationType(model.ParentId, model.AuthenticationType);

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateScreenModel model)
        {
            #region Validate Name Translations

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #region Validate Duplication

            await ValidateScreenNameDuplication(model.NameTranslations);

            #endregion

            #endregion

            await ValidateOrderDuplication(model.Order, model.Id, model.AuthenticationType);

            await ValidateParent(model.ParentId);

            await CheckParentAuthenticationType(model.ParentId, model.AuthenticationType);

            return true;
        }
        private async Task<bool> ValidateScreenNameDuplication(List<NameTranslationModel> nameTranslations)
        {
            foreach (var nameTranslation in nameTranslations)
            {
                var checkNameDuplicate = await repositoryManager.MenuItemNameTranslationRepository.
                    Get(pt => pt.Id != nameTranslation.Id && nameTranslation.Name == pt.Name &&
                    pt.MenuItem.GroupOrScreenType == GroupOrScreenType.Screen &&
                    nameTranslation.LanguageId == pt.LanguageId).
                    Select(l => new
                    {
                        l.Name,
                        LanguageName = l.Language.NativeName
                    }).FirstOrDefaultAsync();

                if (checkNameDuplicate != null)
                {
                    throw new BusinessValidationException(messageCode: null,
                        message: TranslationHelper.GetTranslation(LeillaKeys.SorryScreenNameIsDuplicated, requestInfo.Lang) +
                        LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.DuplicatedScreenName, requestInfo.Lang) + LeillaKeys.ColonsThenSpace +
                        checkNameDuplicate.Name + LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.DuplicatedScreenLanguage, requestInfo.Lang) + LeillaKeys.ColonsThenSpace +
                        checkNameDuplicate.LanguageName);
                }
            }

            return true;
        }
        private async Task<bool> ValidateOrderDuplication(int order, int id, AuthenticationType authenticationType)
        {
            var checkOrderDuplicate = await repositoryManager.MenuItemRepository.
                Get(m => !m.IsDeleted && m.GroupOrScreenType == GroupOrScreenType.Screen &&
                m.AuthenticationType == authenticationType &&
                m.Id != id && m.Order == order).
                Select(screenGroup => new
                {
                    screenGroup.MenuItemNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name
                }).FirstOrDefaultAsync();

            if (checkOrderDuplicate != null)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper.GetTranslation(LeillaKeys.SorryScreenGroupOrderIsDuplicated, requestInfo.Lang) +
                    LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.DuplicatedWithScreenGroupName, requestInfo.Lang) + LeillaKeys.ColonsThenSpace +
                    checkOrderDuplicate.Name);
            }

            return true;
        }
        private async Task<bool> ValidateParent(int? parentId)
        {
            if (parentId > 0)
            {
                var checkParent = await repositoryManager.MenuItemRepository.
                    Get(m => !m.IsDeleted && m.Id == parentId && m.GroupOrScreenType == GroupOrScreenType.Screen).
                    AnyAsync();

                if (checkParent)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryScreenParentMustBeScreenGroup);
                }
            }

            return true;
        }
        private async Task<bool> CheckParentAuthenticationType(int? parentId, AuthenticationType authenticationType)
        {
            if (parentId > 0)
            {
                var checkParent = await repositoryManager.MenuItemRepository.
                    Get(m => !m.IsDeleted && m.Id == parentId && m.AuthenticationType != authenticationType).
                    AnyAsync();

                if (checkParent)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryParentMustBeFromTheSameAuthenticationType);
                }
            }

            return true;
        }
    }
}
