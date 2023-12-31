using Dawem.API.MiddleWares.Helpers;
using Dawem.Contract.BusinessLogic.Permissions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dawem.API.MiddleWares
{
    public class PermissionMiddleWare
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo, IPermissionBL permissionBL, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            var controllerActionDescriptor = httpContext
                    ?.GetEndpoint()
                    ?.Metadata
                    ?.GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerTypeInfo.Name;
            var actionName = controllerActionDescriptor?.ActionName;

            var userId = requestInfo.UserId;

            if (httpContext != null && userId > 0 && !string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(actionName))
            {
                var mapResult = ControllerActionHelper.MapControllerAndAction(controllerName: controllerName, actionName: actionName);
                if (mapResult.Screen != null && mapResult.Method != null)
                {
                    var model = new CheckUserPermissionModel
                    {
                        UserId = userId,
                        Screen = mapResult.Screen.Value,
                        Action = mapResult.Method.Value
                    };

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
                                   requestInfo?.Lang) + LeillaKeys.Space + LeillaKeys.LeftBracket +
                                   TranslationHelper.GetTranslation(mapResult.Method.Value.ToString(),
                                   requestInfo?.Lang) + LeillaKeys.Space + LeillaKeys.RightBracket +
                                   TranslationHelper.GetTranslation(LeillaKeys.InScreen,
                                   requestInfo?.Lang) + LeillaKeys.Space + LeillaKeys.LeftBracket +
                                   TranslationHelper.GetTranslation(mapResult.Screen.Value.ToString() + LeillaKeys.Screen,
                                   requestInfo?.Lang) + LeillaKeys.Space + LeillaKeys.RightBracket
                        };
                        await Return(unitOfWork, httpContext, statusCode, response);
                    }
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }
        private static async Task Return(IUnitOfWork<ApplicationDBContext> unitOfWork, HttpContext context, int statusCode, ErrorResponse response)
        {
            unitOfWork.Rollback();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = LeillaKeys.ApplicationJson;
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
        }
    }
}
