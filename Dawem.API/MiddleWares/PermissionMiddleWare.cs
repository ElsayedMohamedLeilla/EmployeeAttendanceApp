using Dawem.API.Helpers;
using Dawem.Contract.BusinessLogic.Dawem.Permissions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Dawem.API.MiddleWares
{
    public class PermissionMiddleWare
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public List<string> ExcludedApis = new() { LeillaKeys.GetForDropDown, LeillaKeys.GetAllScreens, LeillaKeys.GetAllActions };
        public List<string> ExcludedControllers = new() { LeillaKeys.NotificationController };

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo, IPermissionBL permissionBL, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            var controllerActionDescriptor = httpContext
                    ?.GetEndpoint()
                    ?.Metadata
                    ?.GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerTypeInfo.Name;
            var actionName = controllerActionDescriptor?.ActionName;

            var userId = requestInfo.UserId;

            if (httpContext != null && userId > 0 && 
                !string.IsNullOrWhiteSpace(controllerName) && 
                !string.IsNullOrWhiteSpace(actionName) && 
                !ExcludedControllers.Contains(controllerName) && 
                !ExcludedApis.Contains(actionName))
            {
                var mapResult = ControllerActionHelper.MapControllerAndAction(controllerName: controllerName, actionName: actionName, requestInfo.Type);
                if (mapResult.Screen != null && mapResult.Method != null)
                {
                    var model = new CheckUserPermissionModel
                    {
                        CompanyId = requestInfo.CompanyId,
                        UserId = userId,
                        ScreenCode = mapResult.Screen.Value,
                        ActionCode = mapResult.Method.Value,
                        ActionName = actionName,
                        AuthenticationType = requestInfo.Type
                    };

                    dynamic screenCode = requestInfo.Type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)mapResult.Screen.Value :
                    (DawemAdminApplicationScreenCode)mapResult.Screen.Value;

                    var screenNameSuffix = requestInfo.Type == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

                    var checkPermissionResponse = await permissionBL.CheckUserPermission(model);
                    if (checkPermissionResponse)
                    {
                        await _next.Invoke(httpContext);
                    }
                    else
                    {
                        int statusCode = StatusCodes.Status403Forbidden;
                        var response = new ErrorResponse
                        {
                            State = ResponseStatus.Forbidden,
                            Message = TranslationHelper.GetTranslation(LeillaKeys.SorryYouDoNotHavePermission,
                                   requestInfo.Lang) + LeillaKeys.Space + LeillaKeys.LeftBracket +
                                   TranslationHelper.GetTranslation(mapResult.Method.Value.ToString(),
                                   requestInfo.Lang) + LeillaKeys.RightBracket +
                                   LeillaKeys.Space +
                                   TranslationHelper.GetTranslation(LeillaKeys.InScreen,
                                   requestInfo.Lang) + LeillaKeys.Space + LeillaKeys.LeftBracket +
                                   TranslationHelper.GetTranslation(screenCode.ToString() + screenNameSuffix,
                                   requestInfo.Lang) + LeillaKeys.RightBracket
                        };

                        await ReturnHelper.Return(unitOfWork, httpContext, statusCode, response);
                    }
                }
                else
                {
                    int statusCode = StatusCodes.Status403Forbidden;
                    var response = new ErrorResponse
                    {
                        State = ResponseStatus.Forbidden,
                        Message = TranslationHelper.GetTranslation(LeillaKeys.SorryInternalErrorHappenInPermissions, requestInfo?.Lang)
                    };

                    await ReturnHelper.Return(unitOfWork, httpContext, statusCode, response);
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}
