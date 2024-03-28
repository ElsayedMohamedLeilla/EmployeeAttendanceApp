using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Generic;
using Dawem.Translations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dawem.API.Helpers
{
    public static class ReturnHelper
    {
        public static async Task Return(IUnitOfWork<ApplicationDBContext> unitOfWork, HttpContext context, int statusCode, ErrorResponse response)
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
        public static async Task Return(IUnitOfWork<ApplicationDBContext> unitOfWork, HttpContext context, int statusCode, ErrorResponseGenaric<int> response)
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
