using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Exceptions;
using Dawem.Models.Generic;
using Dawem.Translations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
                await Return(unitOfWork, context, statusCode, response);
            }
            catch (BusinessValidationExceptionGenaric<int> ex)
            {
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                responseGenaric.State = ResponseStatus.ValidationError;
                responseGenaric.Message = !string.IsNullOrEmpty(ex.Message) ? ex.Message :
                     TranslationHelper.GetTranslation(ex.MessageCode, requestInfo?.Lang);
                responseGenaric.Data = ex.Data;
                await Return(unitOfWork, context, statusCode, responseGenaric);
            }
            catch (ActionNotAllowedValidationError ex)
            {
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response.State = ResponseStatus.ActionNotAllowed;
                response.Message = !string.IsNullOrEmpty(ex.Message) ? ex.Message :
                                TranslationHelper.GetTranslation(ex.MessageCode, requestInfo?.Lang);
                await Return(unitOfWork, context, statusCode, response);
            }
            catch (UnAuthorizedException ex)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                response.State = ResponseStatus.UnAuthorized;
                response.Message = string.IsNullOrEmpty(ex.Message) ? LeillaKeys.UnAuthorized : ex.Message;
                await Return(unitOfWork, context, statusCode, response);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (NotRegisteredUserException)
            {
                statusCode = (int)HttpStatusCode.OK;
                response.State = ResponseStatus.NotRegisteredUser;
                response.Message = LeillaKeys.RedirectToRegister;
                await Return(unitOfWork, context, statusCode, response);

            }
            catch (DbUpdateException ex)
            {
                var innerException = (SqlException)ex.InnerException;

                if (innerException != null)
                {
                    response.Message = innerException.Number switch
                    {
                        // Cannot insert duplicate key row in object ........
                        2601 => TranslationHelper.
                        GetTranslation(LeillaKeys.SorryDuplicationOfDataIsNotAllowedPleaseTryAgain, requestInfo?.Lang),

                        // String or binary data would be truncated in table ........
                        2628 => TranslationHelper.
                        GetTranslation(LeillaKeys.SorryTheSizeOfInsertedDataIsBigPleaseDecreaseTheDataSizeAndTryAgain, requestInfo?.Lang),

                        _ => TranslationHelper.
                        GetTranslation(LeillaKeys.SorryErrorHappenWhenProcessingInsertedData, requestInfo?.Lang),
                    };
                }
                else
                {
                    response.Message = TranslationHelper.
                        GetTranslation(LeillaKeys.SorryErrorHappenWhenProcessingInsertedData, requestInfo?.Lang);
                }

                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response.State = ResponseStatus.ValidationError;
                await Return(unitOfWork, context, statusCode, response);

            }
            catch (Exception exception)
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                response.State = ResponseStatus.Error;
                response.Message = TranslationHelper.
                        GetTranslation(LeillaKeys.SorryInternalErrorHappenedPleaseContactDawemSupportToSolveIt, requestInfo?.Lang);
                await Return(unitOfWork, context, statusCode, response);
            }
        }
        private static async Task Return(IUnitOfWork<ApplicationDBContext> unitOfWork, HttpContext context, int statusCode, ErrorResponse response)
        {
            unitOfWork.Rollback();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = LeillaKeys.ApplicationJson;
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
        }
        private static async Task Return(IUnitOfWork<ApplicationDBContext> unitOfWork, HttpContext context, int statusCode, ErrorResponseGenaric<int> response)
        {
            unitOfWork.Rollback();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = LeillaKeys.ApplicationJson;
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
        }
    }
}