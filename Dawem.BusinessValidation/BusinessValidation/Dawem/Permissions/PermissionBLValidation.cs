using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidation.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Permissions
{

    public class PermissionBLValidation : IPermissionBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        private readonly IScreenBLC screenBLC;
        public PermissionBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo, IScreenBLC _screenBLC)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
            screenBLC = _screenBLC;
        }
        public async Task<bool> CreateValidation(CreatePermissionModel model)
        {
            #region Vaildate Permission Screens

            ValidatePermissionScreens(model.Screens);

            #endregion

            if (model.ResponsibilityId != null)
            {
                var checkResponsibility = await repositoryManager.ResponsibilityRepository.Get(responsibility =>
                responsibility.Id == model.ResponsibilityId && !responsibility.IsDeleted &&
                responsibility.Type == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && responsibility.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && responsibility.CompanyId == null))).AnyAsync();
                if (!checkResponsibility)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNotFound);
                }

                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(permission => permission.AuthenticationType == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.ResponsibilityId == model.ResponsibilityId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionResponsibilityIsDuplicated);
                }
            }
            else if (model.UserId != null)
            {
                var checkUser = await repositoryManager.UserRepository.Get(user =>
                user.Id == model.UserId && !user.IsDeleted &&
                user.Type == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && user.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && user.CompanyId == null))).AnyAsync();
                if (!checkUser)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
                }

                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(permission => permission.AuthenticationType == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.UserId == model.UserId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionUserIsDuplicated);
                }
            }

            #region Validate Available Actions

            await ValidatePermissionScreenAvailableActions(model.Screens);

            #endregion

            return true;
        }
        public async Task<bool> UpdateValidation(UpdatePermissionModel model)
        {
            #region Vaildate Permission Screens

            ValidatePermissionScreens(model.Screens);

            #endregion

            if (model.ResponsibilityId != null)
            {
                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(permission => permission.Id != model.Id &&
                permission.AuthenticationType == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.ResponsibilityId == model.ResponsibilityId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionResponsibilityIsDuplicated);
                }
            }
            else if (model.UserId != null)
            {
                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(permission => permission.Id != model.Id && !permission.IsDeleted &&
                permission.AuthenticationType == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.UserId == model.UserId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionUserIsDuplicated);
                }
            }


            #region Validate Available Actions

            await ValidatePermissionScreenAvailableActions(model.Screens);

            #endregion

            return true;
        }
        private void ValidatePermissionScreens(List<PermissionScreenModel> PermissionScreens)
        {
            /*bool check = false;

            if (requestInfo.Type == AuthenticationType.AdminPanel)
            {
                check = PermissionScreens.Any(ps => !Enum.IsDefined(typeof(AdminPanelApplicationScreenCode), ps.ScreenCode));
            }
            else if (requestInfo.Type == AuthenticationType.DawemAdmin)
            {
                check = PermissionScreens.Any(ps => !Enum.IsDefined(typeof(DawemAdminApplicationScreenCode), ps.ScreenCode));
            }

            if (check)
            {
                throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterCorrectScreenCode);
            }*/
        }
        private async Task ValidatePermissionScreenAvailableActions(List<PermissionScreenModel> permissionScreens)
        {
            var screensIds = permissionScreens.Select(s => s.ScreenId).ToList();

            var getScreens = await repositoryManager.MenuItemRepository.
                Get(m => m.IsActive && screensIds.Contains(m.Id) &&
                m.GroupOrScreenType == GroupOrScreenType.Screen).
                Select(m => new
                {
                    m.Id,
                    m.MenuItemNameTranslations.
                        First(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    Actions = m.MenuItemActions.Select(a => a.ActionCode).ToList()
                }).ToListAsync();

            #region Validate Plan Screens

            var getPlan = await repositoryManager.SubscriptionRepository.
                    Get(s => s.CompanyId == requestInfo.CompanyId).
                    Select(c => new
                    {
                        c.Plan.AllScreensAvailable,
                        PlanScreens = c.Plan.PlanScreens != null ? c.Plan.PlanScreens.Select(ps => ps.ScreenId).ToList() : new List<int>()
                    }).FirstOrDefaultAsync();

            if (getPlan != null && !getPlan.AllScreensAvailable && getPlan.PlanScreens != null)
            {
                var allPlanScreens = getPlan.PlanScreens;
                var getScreenId = screensIds?.FirstOrDefault(sId => !allPlanScreens.Contains(sId));

                if (getScreenId != null)
                {
                    var getScreen = getScreens.FirstOrDefault(s => s.Id == getScreenId);

                    if (getScreen != null)
                    {
                        var planMessage = TranslationHelper.GetTranslation(LeillaKeys.SorryYourCurrentSubscriptionPlanDoNotHaveTheRequiredScreen,
                                           requestInfo.Lang) + LeillaKeys.Space +
                                           TranslationHelper.GetTranslation(LeillaKeys.ScreenName,
                                           requestInfo.Lang) + LeillaKeys.Space + LeillaKeys.LeftBracket +
                                           getScreen.Name + LeillaKeys.RightBracket;
                        throw new BusinessValidationException(messageCode: null, message: planMessage);
                    }
                }
            } 

            #endregion

            #region Validate Available Actions

            if (getScreens != null)
            {
                var screenWithNotAvailableAction = permissionScreens
                    .FirstOrDefault(permissionScreen => permissionScreen.Actions
                        .Any(actionCode => !getScreens
                        .FirstOrDefault(s => s.Id == permissionScreen.ScreenId).Actions.Contains(actionCode)));

                if (screenWithNotAvailableAction != null)
                {
                    var screenInfo = getScreens.FirstOrDefault(s => s.Id == screenWithNotAvailableAction.ScreenId);

                    /*dynamic screenCode = requestInfo.Type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)screenWithNotAvailableAction.ScreenId :
                    (DawemAdminApplicationScreenCode)screenWithNotAvailableAction.ScreenId;*/

                    var actionNotAvailable = screenWithNotAvailableAction.Actions
                        .FirstOrDefault(actionCode => !getScreens
                        .FirstOrDefault(s => s.Id == screenWithNotAvailableAction.ScreenId).Actions.Contains(actionCode));

                    /*var screenNameSuffix = requestInfo.Type == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;*/

                    var message = TranslationHelper.GetTranslation(LeillaKeys.SorryChosenActionNotAvailableForChosenScreen, requestInfo.Lang)
                        + LeillaKeys.Space +
                        TranslationHelper.GetTranslation(LeillaKeys.ScreenName, requestInfo.Lang)
                        + screenInfo.Name
                        + LeillaKeys.SpaceThenDashThenSpace +
                        TranslationHelper.GetTranslation(LeillaKeys.ActionName, requestInfo.Lang)
                        + TranslationHelper.GetTranslation(actionNotAvailable.ToString(), requestInfo.Lang)
                        + LeillaKeys.Dot;

                    throw new BusinessValidationException(messageCode: null, message: message);
                }
            }

            #endregion
        }
    }
}
