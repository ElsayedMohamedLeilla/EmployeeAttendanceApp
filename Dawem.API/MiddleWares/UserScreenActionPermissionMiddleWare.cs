using Dawem.API.MiddleWares.Helpers;
using Dawem.Contract.BusinessLogic.Others;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace Dawem.API.MiddleWares
{
    public class UserScreenActionPermissionMiddleWare
    {
        private readonly RequestDelegate _next;

        public UserScreenActionPermissionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestInfo userContext, IUserScreenActionPermissionBL userScreenActionPermissionBL)
        {


            var controllerActionDescriptor = httpContext
                    ?.GetEndpoint()
                    ?.Metadata
                    ?.GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerTypeInfo.Name;
            var actionName = controllerActionDescriptor?.ActionName;



            var userId = 0;
            var authHeaders = httpContext?.Request?.Headers["Authorization"];
            if (authHeaders?.Count() > 0 && authHeaders.Value[0] != null && authHeaders.Value[0].StartsWith("Bearer "))
            {
                var jwt = authHeaders.Value[0]?.Substring(7);
                var jwtSecurityToken = new JwtSecurityToken(jwt);
                var userIdText = jwtSecurityToken?.Claims?.ToArray()?.FirstOrDefault(x => string.Equals(x.Type, "unique_name"))?.Value;
                int.TryParse(userIdText, out userId);
                userContext.UserId = userId;
            }




            if (httpContext != null && userId > 0 && controllerName != null && actionName != null)
            {
                var mapResult = ControllerActionHelper.MapControllerAndAction(controllerName: controllerName, actionName: actionName);
                if (mapResult.Status == ResponseStatus.Success)
                {
                    var model = new CheckUserPermissionModel();
                    model.Screen = mapResult.Screen;
                    model.Method = mapResult.Method;

                    //var checkPermissionResponse = await userScreenActionPermissionBL.CheckUserPermission(model);
                    //if (checkPermissionResponse.Status == Enums.ResponseStatus.Success)
                    //{
                    //    await _next.Invoke(httpContext);
                    //}
                    //else
                    //{
                    //    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(checkPermissionResponse));
                    //}
                }

            }
            else
            {
                await _next.Invoke(httpContext);
            }















            userContext.Lang = HttpRequestHelper.getLangKey(httpContext?.Request);

            //var userIdw = userManager.GetUserId(httpContext.User);



            //// here we read the user info from the Authorization header on the request
            //var authHeaders = httpContext.Request.Headers["Authorization"];
            //// you can load the app before logging in, but you can't access the database
            //if (authHeaders.Count() > 0 && authHeaders[0] != null && authHeaders[0].StartsWith("Bearer "))
            //{
            //    var jwt = authHeaders[0]?.Substring(7);
            //    var jwtSecurityToken = new JwtSecurityToken(jwt);
            //    var userIdText = jwtSecurityToken?.Claims?.ToArray()?.FirstOrDefault(x => string.Equals(x.Type, "unique_name"))?.Value;
            //    int.TryParse(userIdText, out userId);
            //    userContext.UserId = userId;
            //}
            //if (userId > 0)
            //{
            //    //userContext.SmartUser = await userManager.FindByIdAsync(userId.ToString());

            //}


            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith(LeillaKeys.Ar))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(LeillaKeys.En);
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(LeillaKeys.En);
            }


            userContext.RequestProtocol = httpContext.Request.Scheme;
            userContext.RequestHost = httpContext.Request.Host.Host;
            userContext.RequestPort = httpContext.Request.Host.Port;
            userContext.RequestPath = httpContext.Request.Path;
            userContext.BaseUrl = $"{userContext.RequestProtocol}://{userContext.RequestHost}";
            userContext.BaseUrl += userContext.RequestPort.HasValue ? ":" + userContext.RequestPort : LeillaKeys.EmptyString;

            var branchId = HttpRequestHelper.getHeaderKey<int?>(httpContext.Request, "BranchId");
            userContext.BranchId = branchId == 0 ? null : branchId;

            if (branchId > 0)
            {
                //var branch = branchRepository.GetEntityByCondition(b => b.Id == branchId);
                //userContext.IsMainBranch = branch.IsMainBranch;
                //userContext.CompanyId = branch.CompanyId;
            }


        }
    }
}
