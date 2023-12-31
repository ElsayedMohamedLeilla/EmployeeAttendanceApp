using Dawem.Contract.BusinessValidation.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Permissions.Permissions;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Permissions
{

    public class PermissionBLValidation : IPermissionBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public PermissionBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreatePermissionModel model)
        {
            var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.RoleId == model.RoleId).AnyAsync();
            if (checkPermissionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionRoleIsDuplicated);
            }

            #region Validate Available Actions

            var allScreensWithAvailableActions = APIHelper.AllScreensWithAvailableActions;

            if (allScreensWithAvailableActions != null)
            {

                var screenWithNotAvailableAction = model.PermissionScreens
                    .FirstOrDefault(permissionScreen => permissionScreen.PermissionScreenActions
                        .Any(a => !allScreensWithAvailableActions.Screens
                        .FirstOrDefault(s=>s.ScreenCode == permissionScreen.ScreenCode).AvailableActions.Contains(a.ActionCode)));

                if (screenWithNotAvailableAction != null)
                {
                    var actionNotAvailable = screenWithNotAvailableAction.PermissionScreenActions
                        .FirstOrDefault(a => !allScreensWithAvailableActions.Screens
                        .FirstOrDefault(s => s.ScreenCode == screenWithNotAvailableAction.ScreenCode).AvailableActions.Contains(a.ActionCode));

                    var message = TranslationHelper.GetTranslation(LeillaKeys.SorryChosenActionNotAvailableForChosenScreen, requestInfo.Lang)
                        + LeillaKeys.Space +
                        TranslationHelper.GetTranslation(LeillaKeys.ScreenName, requestInfo.Lang)
                        + TranslationHelper.GetTranslation(screenWithNotAvailableAction.ScreenCode.ToString() + LeillaKeys.Screen, requestInfo.Lang)
                        + LeillaKeys.SpaceThenDashThenSpace +
                        TranslationHelper.GetTranslation(LeillaKeys.ActionName, requestInfo.Lang)
                        + TranslationHelper.GetTranslation(actionNotAvailable.ActionCode.ToString(), requestInfo.Lang)
                        + LeillaKeys.Dot;

                    throw new BusinessValidationException(messageCode: null, message: message);
                }
            }

            #endregion

            return true;
        }
        public async Task<bool> UpdateValidation(UpdatePermissionModel model)
        {
            var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.RoleId == model.RoleId && c.Id != model.Id).AnyAsync();
            if (checkPermissionDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryPermissionRoleIsDuplicated);
            }

            #region Validate Available Actions

            var allScreensWithAvailableActions = APIHelper.AllScreensWithAvailableActions;

            if (allScreensWithAvailableActions != null)
            {

                var screenWithNotAvailableAction = model.PermissionScreens
                    .FirstOrDefault(permissionScreen => permissionScreen.PermissionScreenActions
                        .Any(a => !allScreensWithAvailableActions.Screens
                        .FirstOrDefault(s => s.ScreenCode == permissionScreen.ScreenCode).AvailableActions.Contains(a.ActionCode)));

                if (screenWithNotAvailableAction != null)
                {
                    var actionNotAvailable = screenWithNotAvailableAction.PermissionScreenActions
                        .FirstOrDefault(a => !allScreensWithAvailableActions.Screens
                        .FirstOrDefault(s => s.ScreenCode == screenWithNotAvailableAction.ScreenCode).AvailableActions.Contains(a.ActionCode));

                    var message = TranslationHelper.GetTranslation(LeillaKeys.SorryChosenActionNotAvailableForChosenScreen, requestInfo.Lang)
                        + LeillaKeys.Space +
                        TranslationHelper.GetTranslation(LeillaKeys.ScreenName, requestInfo.Lang)
                        + TranslationHelper.GetTranslation(screenWithNotAvailableAction.ScreenCode.ToString() + LeillaKeys.Screen, requestInfo.Lang)
                        + LeillaKeys.Space +
                        TranslationHelper.GetTranslation(LeillaKeys.ActionName, requestInfo.Lang)
                        + TranslationHelper.GetTranslation(actionNotAvailable.ActionCode.ToString(), requestInfo.Lang)
                        + LeillaKeys.Dot;

                    throw new BusinessValidationException(messageCode: null, message: message);
                }
            }

            #endregion

            return true;
        }
    }
}
