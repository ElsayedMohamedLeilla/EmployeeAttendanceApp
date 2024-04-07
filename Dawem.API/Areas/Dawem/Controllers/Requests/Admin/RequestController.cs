using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Models.Requests;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Requests.Admin
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RequestController : DawemControllerBase
    {
        private readonly IRequestBL requestBL;
        public RequestController(IRequestBL _requestBL)
        {
            requestBL = _requestBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetRequestsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestsresponse = await requestBL.Get(criteria);

            return Success(requestsresponse.Requests, requestsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestBL.GetInfo(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Accept(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestBL.Accept(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Reject([FromQuery] RejectModelDTO rejectModelDTO)
        {
            if (rejectModelDTO.Id < 1)
            {
                return BadRequest();
            }
            return Success(await requestBL.Reject(rejectModelDTO));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestBL.Delete(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetRequestsInformations()
        {
            var response = await requestBL.GetRequestsInformations();

            return Success(response);
        }

    }
}