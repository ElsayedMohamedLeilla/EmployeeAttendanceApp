using Dawem.API.MiddleWares.Helpers;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Translations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dawem.API.MiddleWares
{
    public class UnauthorizedMessageHandlerMiddleware
    {
        private readonly RequestDelegate _request;

        public UnauthorizedMessageHandlerMiddleware(RequestDelegate next)
        {
            _request = next;
        }
        public Task Invoke(HttpContext context, RequestInfo userContext, IUnitOfWork<ApplicationDBContext> unitOfWork) => InvokeAsync(context, userContext, unitOfWork);
        async Task InvokeAsync(HttpContext context, RequestInfo requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {

            await _request.Invoke(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                int statusCode = StatusCodes.Status401Unauthorized;
                var response = new ErrorResponse
                {
                    State = ResponseStatus.Forbidden,
                    Message = TranslationHelper.GetTranslation(LeillaKeys.SorryYourAccessDataIsIncorrectPleaseCheckYourUserNameAndPassword,
                           requestInfo?.Lang)
                };
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);

            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                int statusCode = StatusCodes.Status403Forbidden;
                var response = new ErrorResponse
                {
                    State = ResponseStatus.UnAuthorized,
                    Message = TranslationHelper.GetTranslation(LeillaKeys.SorryYouAreForbiddenToAccessRequestedData,
                           requestInfo?.Lang)
                };
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);

            }
        }
        
    }
}