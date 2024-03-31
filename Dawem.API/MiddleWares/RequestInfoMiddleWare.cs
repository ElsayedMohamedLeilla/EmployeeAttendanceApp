using Dawem.Contract.Repository.Manager;
using Dawem.Contract.Repository.Provider;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Repository.UserManagement;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Calendars;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace Dawem.API.MiddleWares
{
    public class RequestInfoMiddleWare
    {
        private readonly RequestDelegate _next;

        public RequestInfoMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo,
            UserManagerRepository userManager, IRepositoryManager repositoryManager,
            ICompanyBranchRepository branchRepository, IOptions<Jwt> appSettings)
        {
            requestInfo.Lang = HttpRequestHelper.getLangKey(httpContext.Request);
            requestInfo.RequestPath = httpContext.Request.Path;

            int userId = 0;
            int companyId = 0;
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
                    requestInfo.LocalHijriDateTime = GetCurrentLocalHijriDateTime();
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
                requestInfo.IsAdminPanel = requestInfo.User.IsForAdminPanel/* && 
                    requestInfo.RequestPath.ToLower().Contains(LeillaKeys.AdminPanel)*/;

                requestInfo.CompanyId = requestInfo.IsAdminPanel ? 0 : requestInfo.CompanyId;
            }

            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith(LeillaKeys.Ar))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(LeillaKeys.En);
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(LeillaKeys.En);
            }

            await _next.Invoke(httpContext);


        }

        public LocalDateTime GetCurrentLocalHijriDateTime()
        {
            var leapYearPattern = IslamicLeapYearPattern.Base15;
            var epoch = IslamicEpoch.Astronomical;
            var hijriCalendar = CalendarSystem.GetIslamicCalendar(leapYearPattern, epoch);
            var now = SystemClock.Instance.GetCurrentInstant();
            var localDateTimeInHijri = now.InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()).WithCalendar(hijriCalendar);
            return localDateTimeInHijri.LocalDateTime;
        }

    }
}
