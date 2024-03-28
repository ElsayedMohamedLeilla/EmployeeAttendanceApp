using Dawem.Contract.BusinessLogic.Requests;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Vacations;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Requests.Admin
{
    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
    public class RequestVacationController : BaseController
    {
        private readonly IRequestVacationBL requestVacationBL;


        public RequestVacationController(IRequestVacationBL _requestVacationBL)
        {
            requestVacationBL = _requestVacationBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateRequestVacationWithImageDTO formData)
        {
            if (formData == null || formData.CreateRequestVacationModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateRequestVacationDTO>(formData.CreateRequestVacationModelString);
            model.Attachments = formData.Attachments;
            var result = await requestVacationBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreateVacationRequestSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateRequestVacationWithImageDTO formData)
        {
            if (formData == null || formData.UpdateRequestVacationModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateRequestVacationDTO>(formData.UpdateRequestVacationModelString);
            model.Attachments = formData.Attachments;
            var result = await requestVacationBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdateVacationRequestSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetRequestVacationsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestVacationsresponse = await requestVacationBL.Get(criteria);

            return Success(requestVacationsresponse.VacationRequests, requestVacationsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetRequestVacationsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestVacationsresponse = await requestVacationBL.GetForDropDown(criteria);

            return Success(requestVacationsresponse.VacationRequests, requestVacationsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestVacationBL.GetInfo(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestVacationBL.GetById(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Accept(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestVacationBL.Accept(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Reject([FromQuery] RejectModelDTO rejectModelDTO)
        {
            if (rejectModelDTO.Id < 1)
            {
                return BadRequest();
            }
            return Success(await requestVacationBL.Reject(rejectModelDTO));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestVacationBL.Delete(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetVacationsInformations()
        {
            var response = await requestVacationBL.GetVacationsInformations();

            return Success(response);
        }
    }
}