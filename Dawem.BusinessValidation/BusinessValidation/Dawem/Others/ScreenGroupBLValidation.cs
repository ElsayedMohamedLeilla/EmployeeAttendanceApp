using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


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
            #region Validate Name Translations

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #region Validate Duplication

            await ValidateScreenGroupDuplication(model.NameTranslations);

            #endregion

            #endregion

            await ValidateOrderDuplication(model.Order, 0);

            await ValidateParent(0, model.ParentId);

            await CheckParentAuthenticationType(model.ParentId, model.AuthenticationType);

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateScreenGroupModel model)
        {
            #region Validate Name Translations

            await nameTranslationBLValidationCore.NameTranslationsValidation(model.NameTranslations);

            #region Validate Duplication

            await ValidateScreenGroupDuplication(model.NameTranslations);

            #endregion

            #endregion

            await ValidateOrderDuplication(model.Order, model.Id);

            await ValidateParent(model.Id, model.ParentId);

            await CheckParentAuthenticationType(model.ParentId, model.AuthenticationType);

            return true;
        }

        private async Task<bool> ValidateScreenGroupDuplication(List<NameTranslationModel> nameTranslations)
        {
            foreach (var nameTranslation in nameTranslations)
            {
                var checkNameDuplicate = await repositoryManager.MenuItemNameTranslationRepository.
                    Get(pt => pt.Id != nameTranslation.Id && nameTranslation.Name == pt.Name &&
                    pt.MenuItem.GroupOrScreenType == GroupOrScreenType.Group &&
                    nameTranslation.LanguageId == pt.LanguageId).
                    Select(l => new
                    {
                        l.Name,
                        LanguageName = l.Language.NativeName
                    }).FirstOrDefaultAsync();

                if (checkNameDuplicate != null)
                {
                    throw new BusinessValidationException(messageCode: null,
                        message: TranslationHelper.GetTranslation(LeillaKeys.SorryScreenGroupNameIsDuplicated, requestInfo.Lang) +
                        LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.DuplicatedScreenGroupName, requestInfo.Lang) + LeillaKeys.ColonsThenSpace +
                        checkNameDuplicate.Name + LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.DuplicatedScreenGroupLanguage, requestInfo.Lang) + LeillaKeys.ColonsThenSpace +
                        checkNameDuplicate.LanguageName);
                }
            }

            return true;
        }
        private async Task<bool> ValidateOrderDuplication(int order, int id)
        {
            var checkOrderDuplicate = await repositoryManager.MenuItemRepository.
                Get(m => !m.IsDeleted && m.GroupOrScreenType == GroupOrScreenType.Group && m.Id != id && m.Order == order).
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
        private async Task<bool> ValidateParent(int id, int? parentId)
        {
            if (parentId > 0)
            {
                var checkParent = await repositoryManager.MenuItemRepository.
                    Get(m => !m.IsDeleted && m.Id == parentId && m.GroupOrScreenType == GroupOrScreenType.Screen).
                    AnyAsync();

                if (checkParent)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryGroupParentMustBeScreenGroup);
                }

                var getGroupChildren = await GetGroupChildren(id, new List<int>());

                if (getGroupChildren.Any(childId => childId == parentId))
                {
                    throw new BusinessValidationException(LeillaKeys.SorryGroupParentCannotBeOneOfItsChildren);
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
        private async Task<List<int>> GetGroupChildren(int id, List<int> childrenIds)
        {
            var getParentChildren = await repositoryManager.MenuItemRepository.
                    Get(g => g.ParentId == id).
                    Select(g => g.Id).
                    ToListAsync();

            if (getParentChildren != null && getParentChildren.Count > 0)
            {
                childrenIds.AddRange(getParentChildren);

                foreach (var getParentChild in getParentChildren)
                {
                    await GetGroupChildren(getParentChild, childrenIds);
                }
            }

            return childrenIds;
        }
    }
}
