using Dawem.Models.Context;
using Dawem.Models.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AdminPanelAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var requestInfo = context.HttpContext.RequestServices.GetService<RequestInfo>();
        if (!requestInfo.IsAdminPanel)
        {
            throw new ForbiddenException(LeillaKeys.SorryYouAreForbiddenToAccessRequestedData);
        }
    }
}