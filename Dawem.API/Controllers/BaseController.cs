using Dawem.Domain.Entities;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Response;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dawem.API.Controllers
{

    [Route(DawemKeys.ApiCcontrollerAction)]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly RequestHeaderContext requestHeaderContext;
        public BaseController()
        {
        }
        public BaseController(RequestHeaderContext _requestHeaderContext)
        {
            requestHeaderContext = _requestHeaderContext;
        }
        [NonAction]

        public void Update<TEntity>(List<TEntity> entityList, InserationMode inserationMode)
        {
            entityList.ForEach(x =>
            {
                Update(x, inserationMode);
            });
        }
        [NonAction]
        public void Update<TEntity>(TEntity entity, InserationMode inserationMode)
        {
            try
            {
                if (entity as IBaseEntity != null)
                {
                    if (inserationMode == InserationMode.Insert)
                    {

                        ((IBaseEntity)entity).AddedDate = DateTime.UtcNow;
                        ((IBaseEntity)entity).AddUserId = string.IsNullOrEmpty(User.Identity.Name) ? null : int.Parse(User.Identity.Name);

                    }
                    else if (inserationMode == InserationMode.Update)
                    {
                        ((IBaseEntity)entity).ModifiedDate = DateTime.UtcNow;
                        ((IBaseEntity)entity).ModifyUserId =
                            string.IsNullOrEmpty(User.Identity.Name) ? null :
                            int.Parse(User.Identity.Name)/*User.FindFirst(ClaimTypes.Name).ToString()*/;

                        if (((IBaseEntity)entity).AddUserId == null)
                        {
                            ((IBaseEntity)entity).AddedDate = DateTime.UtcNow;
                            ((IBaseEntity)entity).AddUserId = string.IsNullOrEmpty(User.Identity.Name) ? null : int.Parse(User.Identity.Name)/*User.FindFirst(ClaimTypes.Name).ToString()*/;

                        }


                    }

                }
            }
            catch (Exception)
            {


            }

        }


        [NonAction]
        public BaseResponseT<bool?> GetExecutionResponseWithModelStateErrors()
        {
            string errorsMsg = DawemKeys.EmptyString;
            BaseResponseT<bool?> response = new BaseResponseT<bool?>();
            response.Status = ResponseStatus.InvalidInput;
            foreach (ModelError error in ModelState.Keys.Select(k => ModelState[k].Errors).First())
            {
                errorsMsg = error.Exception != null ? error.Exception.Message : error.ErrorMessage;
                //TranslationHelper.SetResponseMessages(response, "", errorsMsg);
            }

            return response;
        }

        protected ContentResult Success<T>(T result, int? totalCount = null, string messageCode = DawemKeys.DoneSuccessfully, string message = null)
        {
            var responseMessage = message == null ?
                TranslationHelper.GetTranslation(messageCode, requestHeaderContext?.Lang) :
                message;
            var response = new SuccessResponse<T>(result, totalCount, responseMessage);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string json = JsonConvert.SerializeObject(response, settings);
            return Content(json, DawemKeys.ApplicationJson);
        }
    }
}
