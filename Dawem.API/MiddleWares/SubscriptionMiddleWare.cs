using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Translations;

namespace Dawem.API.MiddleWares
{
    public class SubscriptionMiddleWare
    {
        private readonly RequestDelegate _next;

        public SubscriptionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo,
             IRepositoryManager repositoryManager, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            try
            {
                var getSubscription = await repositoryManager.SubscriptionRepository
                        .GetEntityByConditionAsync(s => s.CompanyId == requestInfo.CompanyId);

                if (getSubscription != null)
                {
                    var isSubscriptionExpired = false;
                    if (DateTime.Now.Date >= getSubscription.EndDate.Date)
                    {
                        var getPlansGracePeriodPercentage = (await repositoryManager.DawemSettingRepository.
                            GetEntityByConditionAsync(d => !d.IsDeleted && d.Type == DawemSettingType.PlansGracePeriodPercentage))?.
                            Integer;

                        var extraDays = 0;

                        if (getPlansGracePeriodPercentage != null)
                        {
                            extraDays = getPlansGracePeriodPercentage.Value * getSubscription.DurationInDays / 100;
                        }

                        var newEndDate = getSubscription.EndDate.AddDays(extraDays).Date;

                        if (DateTime.Now.Date >= newEndDate)
                        {
                            isSubscriptionExpired = true;
                            int statusCode = StatusCodes.Status422UnprocessableEntity;
                            var response = new ErrorResponse
                            {
                                State = ResponseStatus.ValidationError,
                                Message = TranslationHelper.GetTranslation(LeillaKeys.SorryYourSubscriptionOnDawemIsExpiredPleaseContactDawemSupportTeamForRenewal,
                                       requestInfo?.Lang)
                            };
                            await ReturnHelper.Return(unitOfWork, httpContext, statusCode, response);
                        }
                    }
                    if (getSubscription.Status != SubscriptionStatus.Active && !isSubscriptionExpired)
                    {
                        int statusCode = StatusCodes.Status422UnprocessableEntity;
                        var response = new ErrorResponse
                        {
                            State = ResponseStatus.ValidationError,
                            Message = TranslationHelper.GetTranslation(LeillaKeys.SorryYourSubscriptionIsNotActiveRightNowPleaseContactDawemSupportTeamForInquiry,
                                   requestInfo?.Lang)
                        };
                        await ReturnHelper.Return(unitOfWork, httpContext, statusCode, response);
                    }

                }
            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
            }
            if (!httpContext.Response.HasStarted)
                await _next.Invoke(httpContext);
        }
    }
}
