using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Justifications;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Requests.Admin
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class RequestJustificationController : BaseController
    {
        private readonly IRequestJustificationBL requestJustificationBL;


        public RequestJustificationController(IRequestJustificationBL _requestJustificationBL)
        {
            requestJustificationBL = _requestJustificationBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateRequestJustificationWithImageDTO formData)
        {
            if (formData == null || formData.CreateRequestJustificationModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateRequestJustificationDTO>(formData.CreateRequestJustificationModelString);
            model.Attachments = formData.Attachments;
            var result = await requestJustificationBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreateJustificationRequestSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateRequestJustificationWithImageDTO formData)
        {
            if (formData == null || formData.UpdateRequestJustificationModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateRequestJustificationDTO>(formData.UpdateRequestJustificationModelString);
            model.Attachments = formData.Attachments;
            var result = await requestJustificationBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdateJustificationRequestSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetRequestJustificationCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestJustificationsresponse = await requestJustificationBL.Get(criteria);

            return Success(requestJustificationsresponse.JustificationRequests, requestJustificationsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetRequestJustificationCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestJustificationsresponse = await requestJustificationBL.GetForDropDown(criteria);

            return Success(requestJustificationsresponse.JustificationRequests, requestJustificationsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestJustificationBL.GetInfo(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestJustificationBL.GetById(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Accept(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestJustificationBL.Accept(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Reject([FromQuery] RejectModelDTO rejectModelDTO)
        {
            if (rejectModelDTO.Id < 1)
            {
                return BadRequest();
            }
            return Success(await requestJustificationBL.Reject(rejectModelDTO));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestJustificationBL.Delete(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetJustificationsInformations()
        {
            var response = await requestJustificationBL.GetJustificationsInformations();

            return Success(response);
        }

    }
}