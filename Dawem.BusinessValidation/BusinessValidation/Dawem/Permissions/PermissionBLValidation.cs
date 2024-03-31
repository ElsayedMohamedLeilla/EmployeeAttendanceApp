using Dawem.Contract.BusinessValidation.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Models.Generic.Exceptions;
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
            if (model.ResponsibilityId != null)
            {
                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.ResponsibilityId == model.ResponsibilityId).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionResponsibilityIsDuplicated);
                }
            }
            else if (model.UserId != null)
            {
                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.UserId == model.UserId).AnyAsync();
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
            if (model.ResponsibilityId != null)
            {
                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.ResponsibilityId == model.ResponsibilityId && c.Id != model.Id).AnyAsync();
                if (checkPermissionDuplicate)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryPermissionResponsibilityIsDuplicated);
                }
            }
            else if (model.UserId != null)
            {
                var checkPermissionDuplicate = await repositoryManager
                .PermissionRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.UserId == model.UserId && c.Id != model.Id).AnyAsync();
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

        private void ValidatePermissionScreenAvailableActions(List<PermissionScreenModel> permissionScreens)
        {
            #region Validate Available Actions

            var allScreensWithAvailableActions = APIHelper.AllScreensWithAvailableActions;

            if (allScreensWithAvailableActions != null)
            {

                var screenWithNotAvailableAction = permissionScreens
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
