using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System.Net;

namespace Dawem.API
{
    public class FluentValidationResultFactory : IFluentValidationAutoValidationResultFactory
    {
        private readonly RequestInfo requestInfo;
        public FluentValidationResultFactory(RequestInfo _requestInfo)
        {
            requestInfo = _requestInfo;
        }
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails validationProblemDetails)
        {
            var errorsInModelState = context.ModelState.
                Where(x => x.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

            var errorResponse = new ModelValidationErrorResponse();
            foreach (var error in errorsInModelState)
            {
                foreach (var subError in error.Value)
                {
                    var errorModel = new ErrorModel
                    {
                        FieldName = !string.IsNullOrEmpty(error.Key) ?
                        error.Key.ToCamelCase() : LeillaKeys.EmptyString,
                        Message = subError
                    };
                    if (string.IsNullOrEmpty(errorResponse.Message))
                    {
                        errorResponse.Message = TranslationHelper.GetTranslation(subError, requestInfo?.Lang);
                    }
                    errorResponse.Errors.Add(errorModel);
                }
            }
            errorResponse.State = ResponseStatus.ValidationError;
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var response = new JsonResult(errorResponse, settings)
            {
                StatusCode = (int)HttpStatusCode.UnprocessableEntity
            };

            return response;

        }
    }
}
