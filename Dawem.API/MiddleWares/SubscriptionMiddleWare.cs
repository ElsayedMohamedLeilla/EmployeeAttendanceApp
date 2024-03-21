using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
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
             IRepositoryManager repositoryManager,
             IUnitOfWork<ApplicationDBContext> unitOfWork, ISubscriptionBLValidationCore subscriptionBLValidationCore)
        {
            try
            {
                var model = new CheckCompanySubscriptionModel
                {
                    CompanyId = requestInfo.CompanyId,
                    FromType = CheckCompanySubscriptionFromType.SubscriptionMiddleWare
                };

                var checkCompanySubscriptionResponse = await subscriptionBLValidationCore.CheckCompanySubscription(model);
                if (checkCompanySubscriptionResponse != null)
                {
                    if (checkCompanySubscriptionResponse.Result)
                    {
                        await _next.Invoke(httpContext);
                    }
                    else
                    {
                        int statusCode = StatusCodes.Status422UnprocessableEntity;
                        var messageCode = string.Empty;

                        switch (checkCompanySubscriptionResponse.ErrorType)
                        {
                            case CheckCompanySubscriptionErrorType.SubscriptionExpired:

                                messageCode = LeillaKeys.SorryYourSubscriptionOnDawemIsExpiredPleaseContactDawemSupportTeamForRenewal;

                                break;
                            case CheckCompanySubscriptionErrorType.SubscriptionNotActive:

                                messageCode = LeillaKeys.SorryYourSubscriptionIsNotActiveRightNowPleaseContactDawemSupportTeamForInquiry;

                                break;
                            default:
                                break;
                        }

                        var response = new ErrorResponse
                        {
                            State = ResponseStatus.ValidationError,
                            Message = TranslationHelper.GetTranslation(messageCode, requestInfo?.Lang)
                        };
                        await ReturnHelper.Return(unitOfWork, httpContext, statusCode, response);
                    }
                }
                else
                {
                    await _next.Invoke(httpContext);
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }
    }
}
