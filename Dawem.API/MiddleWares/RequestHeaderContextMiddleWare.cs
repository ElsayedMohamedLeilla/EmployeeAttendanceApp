using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Translations;
using Microsoft.Extensions.Options;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Generic;
using SmartBusinessERP.Repository.Provider.Contract;
using SmartBusinessERP.Repository.UserManagement;
using SmartBusinessERP.Repository.UserManagement.Contract;
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

        public async Task Invoke(HttpContext httpContext, RequestHeaderContext userContext, SmartUserManagerRepository userManager, ISmartUserRepository smartUserRepository, IBranchRepository branchRepository, IOptions<Jwt> appSettings)
        {
            userContext.Lang = HttpRequestHelper.getLangKey(httpContext.Request);

            var userId = 0;
            var branchId = 0;

            try
            {


                string token = httpContext.Request.Headers[DawemKeys.Authorization];
                if (!string.IsNullOrEmpty(token))
                {
                    var tok = token.Replace(DawemKeys.Bearer, DawemKeys.EmptyString);
                    var jwttoken = new JwtSecurityTokenHandler().ReadJwtToken(tok);

                    var userIdText = jwttoken.Claims.First(claim => claim.Type == "UserId")?.Value;
                    var branchIdText = jwttoken.Claims.First(claim => claim.Type == "BranchId")?.Value;

                    int.TryParse(userIdText.ToString(), out userId);
                    int.TryParse(branchIdText.ToString(), out branchId);

                    userContext.UserId = userId;
                    userContext.BranchId = branchId;
                }

            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
            }


            if (userId > 0)
            {
                userContext.User = await userManager.FindByIdAsync(userId.ToString());

            }

            if (branchId > 0)
            {
                var branch = branchRepository.GetEntityByCondition(b => b.Id == branchId);
                userContext.IsMainBranch = branch.IsMainBranch;
                userContext.CompanyId = branch.CompanyId;
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
