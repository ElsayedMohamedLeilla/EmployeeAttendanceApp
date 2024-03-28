using Dawem.API.Helpers;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Context;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Dawem.API.MiddleWares
{
    public class PermissionLogMiddleWare
    {
        private readonly RequestDelegate _next;

        public PermissionLogMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork, IRepositoryManager repositoryManager)
        {

            await _next.Invoke(context: httpContext);

            var userId = requestInfo.UserId;
            var companyId = requestInfo.CompanyId;

            var controllerActionDescriptor = httpContext
                    ?.GetEndpoint()
                    ?.Metadata
                    ?.GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerTypeInfo.Name;
            var actionName = controllerActionDescriptor?.ActionName;


            if (userId > 0 && companyId > 0 && controllerName != null && actionName != null)
            {
                var mapResult = ControllerActionHelper.MapControllerAndAction(controllerName: controllerName, actionName: actionName);
                if (mapResult.Screen != null && mapResult.Method != null)
                {
                    var permissionLogRepository = repositoryManager.PermissionLogRepository;

                    var permissionLog = new PermissionLog
                    {
                        CompanyId = companyId,
                        UserId = userId,
                        ActionCode = mapResult.Method.Value,
                        ScreenCode = mapResult.Screen.Value,
                    };

                    permissionLogRepository.Insert(permissionLog);
                    _ = unitOfWork.SaveAsync();
                }
            }
        }
    }
}
