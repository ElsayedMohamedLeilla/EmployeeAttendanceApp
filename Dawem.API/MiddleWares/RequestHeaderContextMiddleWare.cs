using Dawem.Contract.Repository.Manager;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Exceptions;
using Dawem.Models.Generic;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
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

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo,
            UserManagerRepository userManager, IRepositoryManager repositoryManager, 
            IBranchRepository branchRepository, IOptions<Jwt> appSettings)
        {
            requestInfo.Lang = HttpRequestHelper.getLangKey(httpContext.Request);

            var userId = 0;
            var companyId = 0;
            int applicationType = 0;

            try
            {


                string token = httpContext.Request.Headers[LeillaKeys.Authorization];
                if (!string.IsNullOrEmpty(token))
                {
                    var tok = token.Replace(LeillaKeys.Bearer, LeillaKeys.EmptyString);
                    var jwttoken = new JwtSecurityTokenHandler().ReadJwtToken(tok);

                    var userIdText = jwttoken.Claims.First(claim => claim.Type == LeillaKeys.UserId)?.Value;
                    var companyIdText = jwttoken.Claims.First(claim => claim.Type == LeillaKeys.CompanyId)?.Value;
                    var applicationTypeText = jwttoken.Claims.First(claim => claim.Type == LeillaKeys.ApplicationType)?.Value;

                    int.TryParse(userIdText.ToString(), out userId);
                    int.TryParse(companyIdText.ToString(), out companyId);
                    int.TryParse(applicationTypeText.ToString(), out applicationType);

                    requestInfo.UserId = userId;
                    requestInfo.CompanyId = companyId;
                    requestInfo.ApplicationType = (ApplicationType)applicationType;


                    var currentCompanyId = requestInfo.CompanyId;

                    var getTimeZoneId = await repositoryManager.CompanyRepository
                        .Get(c => !c.IsDeleted && c.Id == currentCompanyId)
                        .Select(c => c.Country.TimeZoneId)
                        .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryTimeZoneNotFound);

                    requestInfo.LocalDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, getTimeZoneId).DateTime;
                    requestInfo.LocalHijriDateTime = GetHijri(requestInfo.LocalDateTime);
                }

            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
            }


            if (userId > 0)
            {
                requestInfo.User = await userManager.FindByIdAsync(userId.ToString());
                requestInfo.EmployeeId = requestInfo.User.EmployeeId ?? 0;
            }

            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith(LeillaKeys.Ar))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(LeillaKeys.En);
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(LeillaKeys.En);
            }

            await _next.Invoke(httpContext);


        }

        private DateTime GetHijri(DateTime gregorianDate)
        {
            // Convert the Gregorian date to the Hijri calendar
            CultureInfo hijriCulture = new CultureInfo("ar-SA");
            HijriCalendar hijriCalendar = new HijriCalendar();

            DateTimeFormatInfo dateTimeFormat = hijriCulture.DateTimeFormat;
            dateTimeFormat.Calendar = hijriCalendar;

            string hijriDate = gregorianDate.ToString("dd-MM-yyyy", dateTimeFormat);

            // Parse the Hijri date back to DateTime
            DateTime Output =  DateTime.ParseExact(hijriDate, "dd-MM-yyyy", hijriCulture);
            return Output;
        }
    }
}
