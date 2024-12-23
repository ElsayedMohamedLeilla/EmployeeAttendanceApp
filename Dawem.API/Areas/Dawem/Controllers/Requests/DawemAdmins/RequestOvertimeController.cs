using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Justifications;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Requests.DawemAdmins
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class RequestOvertimeController : DawemControllerBase
    {
        private readonly IRequestOvertimeBL requestOvertimeBL;


        public RequestOvertimeController(IRequestOvertimeBL _requestOvertimeBL)
        {
            requestOvertimeBL = _requestOvertimeBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateRequestOvertimeWithImageDTO formData)
        {
            if (formData == null || formData.CreateRequestOvertimeModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateRequestOvertimeDTO>(formData.CreateRequestOvertimeModelString);
            model.Attachments = formData.Attachments;
            var result = await requestOvertimeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateOvertimeRequestSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateRequestOvertimeWithImageDTO formData)
        {
            if (formData == null || formData.UpdateRequestOvertimeModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateRequestOvertimeDTO>(formData.UpdateRequestOvertimeModelString);
            model.Attachments = formData.Attachments;
            var result = await requestOvertimeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateOvertimeRequestSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetRequestOvertimeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestOvertimesresponse = await requestOvertimeBL.Get(criteria);

            return Success(requestOvertimesresponse.OvertimeRequests, requestOvertimesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetRequestOvertimeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestOvertimesresponse = await requestOvertimeBL.GetForDropDown(criteria);

            return Success(requestOvertimesresponse.OvertimeRequests, requestOvertimesresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestOvertimeBL.GetInfo(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestOvertimeBL.GetById(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Accept(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestOvertimeBL.Accept(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Reject([FromQuery] RejectModelDTO rejectModelDTO)
        {
            if (rejectModelDTO.Id < 1)
            {
                return BadRequest();
            }
            return Success(await requestOvertimeBL.Reject(rejectModelDTO));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestOvertimeBL.Delete(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetOvertimesInformations()
        {
            var response = await requestOvertimeBL.GetOvertimesInformations();

            return Success(response);
        }

    }
}