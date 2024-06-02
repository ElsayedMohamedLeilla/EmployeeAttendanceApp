using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidation.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
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
                .PermissionRepository.Get(permission => permission.Type == requestInfo.AuthenticationType &&
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
                .PermissionRepository.Get(permission => permission.Type == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.UserId == model.UserId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionUserIsDuplicated);
                }
            }

            #region Validate Available Actions

            ValidatePermissionScreenAvailableActions(model.Screens);

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
                permission.Type == requestInfo.AuthenticationType &&
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
                permission.Type == requestInfo.AuthenticationType &&
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
            #region Validate Available Actions


            var allScreensWithAvailableActionsGrouped = await screenBLC.
                GetAllScreensWithAvailableActions(new GetScreensCriteria { IsActive = true});

            var allScreensWithAvailableActions = allScreensWithAvailableActionsGrouped.
                MenuItemsTypes.SelectMany(s => s.MenuItems).ToList();

                    /*requestInfo.Type == AuthenticationType.AdminPanel ?
                        APIHelper.AdminPanelAllScreensWithAvailableActions :
                        APIHelper.AllScreensWithAvailableActions*/;

            if (allScreensWithAvailableActions != null)
            {

                var screenWithNotAvailableAction = permissionScreens
                    .FirstOrDefault(permissionScreen => permissionScreen.Actions
                        .Any(actionCode => !allScreensWithAvailableActions
                        .FirstOrDefault(s => s.Id == permissionScreen.ScreenId).AvailableActions.Contains(actionCode)));

                var screenInfo = allScreensWithAvailableActions.FirstOrDefault(s=>s.Id ==  screenWithNotAvailableAction.ScreenId);

                if (screenWithNotAvailableAction != null)
                {
                    /*dynamic screenCode = requestInfo.Type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)screenWithNotAvailableAction.ScreenId :
                    (DawemAdminApplicationScreenCode)screenWithNotAvailableAction.ScreenId;*/

                    var actionNotAvailable = screenWithNotAvailableAction.Actions
                        .FirstOrDefault(actionCode => !allScreensWithAvailableActions
                        .FirstOrDefault(s => s.Id == screenWithNotAvailableAction.ScreenId).AvailableActions.Contains(actionCode));

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
