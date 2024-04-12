using Dawem.Contract.BusinessValidation.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Permissions
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
            #region Vaildate Permission Screens

            ValidatePermissionScreens(model.PermissionScreens);

            #endregion

            if (model.ResponsibilityId != null)
            {
                var checkResponsibility = await repositoryManager.ResponsibilityRepository.Get(responsibility =>
                responsibility.Id == model.ResponsibilityId && !responsibility.IsDeleted &&
                responsibility.Type == requestInfo.Type &&
                ((requestInfo.CompanyId > 0 && responsibility.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && responsibility.CompanyId == null))).AnyAsync();
                if (!checkResponsibility)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryResponsibilityNotFound);
                }

                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(permission => permission.Type == requestInfo.Type &&
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
                user.Type == requestInfo.Type &&
                ((requestInfo.CompanyId > 0 && user.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && user.CompanyId == null))).AnyAsync();
                if (!checkUser)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);
                }

                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(permission => permission.Type == requestInfo.Type &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.UserId == model.UserId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionUserIsDuplicated);
                }
            }

            #region Validate Available Actions

            ValidatePermissionScreenAvailableActions(model.PermissionScreens);

            #endregion

            return true;
        }
        public async Task<bool> UpdateValidation(UpdatePermissionModel model)
        {
            #region Vaildate Permission Screens

            ValidatePermissionScreens(model.PermissionScreens);

            #endregion

            if (model.ResponsibilityId != null)
            {
                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(permission => permission.Id != model.Id &&
                permission.Type == requestInfo.Type &&
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
                permission.Type == requestInfo.Type &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.UserId == model.UserId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionUserIsDuplicated);
                }
            }


            #region Validate Available Actions

            ValidatePermissionScreenAvailableActions(model.PermissionScreens);

            #endregion

            return true;
        }
        private void ValidatePermissionScreens(List<PermissionScreenModel> PermissionScreens)
        {
            bool check = false;

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
            }
        }
        private void ValidatePermissionScreenAvailableActions(List<PermissionScreenModel> permissionScreens)
        {
            #region Validate Available Actions

            var allScreensWithAvailableActions = requestInfo.Type == AuthenticationType.AdminPanel ?
                APIHelper.AdminPanelAllScreensWithAvailableActions :
                APIHelper.AllScreensWithAvailableActions;

            if (allScreensWithAvailableActions != null)
            {

                var screenWithNotAvailableAction = permissionScreens
                    .FirstOrDefault(permissionScreen => permissionScreen.PermissionScreenActions
                        .Any(a => !allScreensWithAvailableActions.Screens
                        .FirstOrDefault(s => s.ScreenCode == permissionScreen.ScreenCode).AvailableActions.Contains(a.ActionCode)));

                if (screenWithNotAvailableAction != null)
                {
                    dynamic screenCode = requestInfo.Type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)screenWithNotAvailableAction.ScreenCode :
                    (DawemAdminApplicationScreenCode)screenWithNotAvailableAction.ScreenCode;

                    var actionNotAvailable = screenWithNotAvailableAction.PermissionScreenActions
                        .FirstOrDefault(a => !allScreensWithAvailableActions.Screens
                        .FirstOrDefault(s => s.ScreenCode == screenWithNotAvailableAction.ScreenCode).AvailableActions.Contains(a.ActionCode));

                    var screenNameSuffix = requestInfo.Type == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

                    var message = TranslationHelper.GetTranslation(LeillaKeys.SorryChosenActionNotAvailableForChosenScreen, requestInfo.Lang)
                        + LeillaKeys.Space +
                        TranslationHelper.GetTranslation(LeillaKeys.ScreenName, requestInfo.Lang)
                        + TranslationHelper.GetTranslation(screenCode.ToString() + screenNameSuffix, requestInfo.Lang)
                        + LeillaKeys.SpaceThenDashThenSpace +
                        TranslationHelper.GetTranslation(LeillaKeys.ActionName, requestInfo.Lang)
                        + TranslationHelper.GetTranslation(actionNotAvailable.ActionCode.ToString(), requestInfo.Lang)
                        + LeillaKeys.Dot;

                    throw new BusinessValidationException(messageCode: null, message: message);
                }
            }

            #endregion
        }
    }
}
