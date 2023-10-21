using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace Dawem.API.MiddleWares
{
    public class RequestHeaderContextMiddleWare
    {
        private readonly RequestDelegate _next;

        public RequestHeaderContextMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo, UserManagerRepository userManager, IUserRepository smartUserRepository, IBranchRepository branchRepository, IOptions<Jwt> appSettings)
        {
            requestInfo.Lang = HttpRequestHelper.getLangKey(httpContext.Request);

            var userId = 0;
            var companyId = 0;
            int applicationType = 0;

            try
            {


                string token = httpContext.Request.Headers[DawemKeys.Authorization];
                if (!string.IsNullOrEmpty(token))
                {
                    var tok = token.Replace(DawemKeys.Bearer, DawemKeys.EmptyString);
                    var jwttoken = new JwtSecurityTokenHandler().ReadJwtToken(tok);

                    var userIdText = jwttoken.Claims.First(claim => claim.Type == DawemKeys.UserId)?.Value;
                    var companyIdText = jwttoken.Claims.First(claim => claim.Type == DawemKeys.CompanyId)?.Value;
                    var applicationTypeText = jwttoken.Claims.First(claim => claim.Type == DawemKeys.ApplicationType)?.Value;

                    int.TryParse(userIdText.ToString(), out userId);
                    int.TryParse(companyIdText.ToString(), out companyId);
                    int.TryParse(applicationTypeText.ToString(), out applicationType);

                    requestInfo.UserId = userId;
                    requestInfo.CompanyId = companyId;
                    requestInfo.ApplicationType = (ApplicationType)applicationType;
                }

            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
            }


            if (userId > 0)
            {
                requestInfo.User = await userManager.FindByIdAsync(userId.ToString());

            }

            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith(DawemKeys.Ar))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(DawemKeys.En);
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(DawemKeys.En);
            }

            await _next.Invoke(httpContext);


        }
    }
}
