using Dawem.Data.UnitOfWork;
using Dawem.Data;
using Dawem.Enums.General;
using DevExpress.Xpo;
using DocumentFormat.OpenXml.InkML;
using Glamatek.Data;
using Glamatek.Data.UnitOfWork;
using Glamatek.Enums;
using Glamatek.Model.Support;
using Glamatek.Utils.Exceptions;
using Glamatek.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using SmartBusinessERP.Models.Context;
using Dawem.Translations;
using Dawem.Models.Exception;

namespace Glamatek.API.MiddleWares;

public class ExceptionHandlerMiddleware
{
    private const string JsonContentType = "application/json";
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
        catch (BusinessValidationErrorException ex)
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
                       userContext?.RequestParam?.Lang)
            };
            await Return(unitOfWork, context, statusCode, response);
        }
        catch (UnAuthorizedException ex)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
            response = new ExecutionResponse<object>
            {
                Message = ex.Message ?? "UnAuthorized"
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
                Message = "Redirect to register"
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

    private async Task Return(IUnitOfWork<ApplicationDbContext> unitOfWork, HttpContext context, int statusCode, ExecutionResponse<object> response)
    {
        unitOfWork.Rollback();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }


}
