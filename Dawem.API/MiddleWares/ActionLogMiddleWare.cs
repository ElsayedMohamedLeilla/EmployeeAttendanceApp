using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using SmartBusinessERP.API.MiddleWares.Helpers;
using SmartBusinessERP.Models.Context;

namespace SmartBusinessERP.API.MiddleWares
{
    public class ActionLogMiddleWare
    {
        private readonly RequestDelegate _next;

        public ActionLogMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestHeaderContext userContext, IUnitOfWork<ApplicationDBContext> unitOfWork, IActionLogBL actionLogBL)
        {


            var originalBody = httpContext.Response.Body;
            using var newBody = new MemoryStream();
            httpContext.Response.Body = newBody;

            try
            {
                await _next.Invoke(context: httpContext);
            }
            finally
            {
                newBody.Seek(0, SeekOrigin.Begin);
                var bodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

                if (!string.IsNullOrEmpty(bodyText) && !string.IsNullOrWhiteSpace(bodyText))
                {
                    var baseResponse = JsonConvert.DeserializeObject<SearchResult<string>>(bodyText);

                    if (baseResponse != null)
                    {
                        var userId = userContext.UserId;
                        var branchId = userContext.BranchId;

                        var controllerActionDescriptor = httpContext
                                ?.GetEndpoint()
                                ?.Metadata
                                ?.GetMetadata<ControllerActionDescriptor>();

                        var controllerName = controllerActionDescriptor?.ControllerTypeInfo.Name;
                        var actionName = controllerActionDescriptor?.ActionName;


                        if (userId > 0 && branchId > 0 && controllerName != null && actionName != null)
                        {
                            var mapResult = ControllerActionHelper.MapControllerAndAction(controllerName: controllerName, actionName: actionName);
                            if (mapResult.Status > 0 && mapResult.Method != null && mapResult.Screen != null)
                            {
                                var model = new CreateActionLogModel();
                                model.ActionType = mapResult.Method ?? 0;
                                model.ActionPlace = mapResult.Screen ?? 0;
                                model.ResponseStatus = mapResult.Status;

                                await actionLogBL.Create(model);
                                await unitOfWork.SaveAsync();
                            }

                        }
                    }


                }

                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
            }
        }
    }
}
