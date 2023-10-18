using Azure;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Exceptions;
using Dawem.Models.Response;
using Dawem.Translations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dawem.API.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private const string JsonContentType = DawemKeys.ApplicationJson;
        private readonly RequestDelegate _request;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _request = next;
        }
        public Task Invoke(HttpContext context, RequestHeaderContext userContext, IUnitOfWork<ApplicationDBContext> unitOfWork) => InvokeAsync(context, userContext, unitOfWork);

        async Task InvokeAsync(HttpContext context, RequestHeaderContext userContext, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            var response = new ExecutionResponse<object>();
            int statusCode = 500;

            try
            {

                await _request(context);
            }
            catch (BusinessValidationException ex)
            {
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response = new ExecutionResponse<object>
                {
                    Result = null,
                    State = ResponseStatus.ValidationError,
                    MessageCode = ex.MessageCode,
                    Message = !string.IsNullOrEmpty(ex.Message) &&
                           !string.IsNullOrWhiteSpace(ex.Message) ?
                           ex.Message : TranslationHelper.GetTranslation(ex.MessageCode,
                           userContext?.Lang)
                };
                await Return(unitOfWork, context, statusCode, response);
            }
            catch (ActionNotAllowedValidationError ex)
            {
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response = new ExecutionResponse<object>
                {
                    Result = null,
                    State = ResponseStatus.ActionNotAllowed,
                    MessageCode = ex.MessageCode,
                    Message = !string.IsNullOrEmpty(ex.Message) &&
                           !string.IsNullOrWhiteSpace(ex.Message) ?
                           ex.Message : TranslationHelper.GetTranslation(ex.MessageCode,
                           userContext?.Lang)
                };
                await Return(unitOfWork, context, statusCode, response);
            }
            catch (UnAuthorizedException ex)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                response = new ExecutionResponse<object>
                {
                    Message = ex.Message ?? DawemKeys.UnAuthorized
                };
                await Return(unitOfWork, context, statusCode, response);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (NotRegisteredUserException)
            {
                statusCode = (int)HttpStatusCode.OK;
                response = new ExecutionResponse<object>
                {
                    Result = null,
                    State = ResponseStatus.NotRegisteredUser,
                    Message = DawemKeys.RedirectToRegister
                };
                await Return(unitOfWork, context, statusCode, response);

            }
            catch (Exception exception)
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                response = new ExecutionResponse<object>
                {
                    Result = null,
                    State = ResponseStatus.Error,
                    Message = exception.Message
                };
                await Return(unitOfWork, context, statusCode, response);
            }
        }

        [Produces("application/json")]
        private static async Task<ActionResult> Return(IUnitOfWork<ApplicationDBContext> unitOfWork, HttpContext context, int statusCode, ExecutionResponse<object> response)
        {
            unitOfWork.Rollback();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = DawemKeys.ApplicationJson;
            //return  await context.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8);
            // return await JsonResult(response);
        }


    }
}