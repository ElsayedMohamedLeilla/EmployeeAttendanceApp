using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Exceptions;
using Dawem.Models.Generic;
using Dawem.Translations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Dawem.API.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _request;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _request = next;
        }
        public Task Invoke(HttpContext context, RequestInfo userContext, IUnitOfWork<ApplicationDBContext> unitOfWork) => InvokeAsync(context, userContext, unitOfWork);
        async Task InvokeAsync(HttpContext context, RequestInfo requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            var response = new ErrorResponse();
            var responseGenaric = new ErrorResponseGenaric<int>();
            int statusCode = 500;

            try
            {
                await _request.Invoke(context);
            }
            catch (BusinessValidationException ex)
            {
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response.State = ResponseStatus.ValidationError;
                response.Message = !string.IsNullOrEmpty(ex.Message) ? ex.Message :
                     TranslationHelper.GetTranslation(ex.MessageCode, requestInfo?.Lang);
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);
            }
            catch (BusinessValidationExceptionGenaric<int> ex)
            {
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                responseGenaric.State = ResponseStatus.ValidationError;
                responseGenaric.Message = !string.IsNullOrEmpty(ex.Message) ? ex.Message :
                     TranslationHelper.GetTranslation(ex.MessageCode, requestInfo?.Lang);
                responseGenaric.Data = ex.Data;
                await ReturnHelper.Return(unitOfWork, context, statusCode, responseGenaric);
            }
            catch (ActionNotAllowedValidationError ex)
            {
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response.State = ResponseStatus.ActionNotAllowed;
                response.Message = !string.IsNullOrEmpty(ex.Message) ? ex.Message :
                                TranslationHelper.GetTranslation(ex.MessageCode, requestInfo?.Lang);
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);
            }
            catch (UnAuthorizedException ex)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                response.State = ResponseStatus.UnAuthorized;
                response.Message = string.IsNullOrEmpty(ex.Message) ? LeillaKeys.UnAuthorized : ex.Message;
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (NotRegisteredUserException)
            {
                statusCode = (int)HttpStatusCode.OK;
                response.State = ResponseStatus.NotRegisteredUser;
                response.Message = LeillaKeys.RedirectToRegister;
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);

            }
            catch (Exception exception)
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                response.State = ResponseStatus.Error;
                response.Message = exception.Message;
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);
            }
        }
       
    }
}