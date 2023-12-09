using Dawem.Contract.BusinessLogic.Requests;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Assignments;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Controllers.Requests
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class EmployeeRequestAssignmentController : BaseController
    {
        private readonly IRequestAssignmentBL requestAssignmentBL;


        public EmployeeRequestAssignmentController(IRequestAssignmentBL _requestAssignmentBL)
        {
            requestAssignmentBL = _requestAssignmentBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateRequestAssignmentWithImageModel formData)
        {
            if (formData == null || formData.CreateRequestAssignmentModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateRequestAssignmentModelDTO>(formData.CreateRequestAssignmentModelString);
            model.Attachments = formData.Attachments;
            var result = await requestAssignmentBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateAssignmentRequestSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateRequestAssignmentWithImageModel formData)
        {
            if (formData == null || formData.UpdateRequestAssignmentModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateRequestAssignmentModelDTO>(formData.UpdateRequestAssignmentModelString);
            model.Attachments = formData.Attachments;
            var result = await requestAssignmentBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateAssignmentRequestSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetRequestAssignmentsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestAssignmentsresponse = await requestAssignmentBL.Get(criteria);

            return Success(requestAssignmentsresponse.AssignmentRequests, requestAssignmentsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetRequestAssignmentsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestAssignmentsresponse = await requestAssignmentBL.GetForDropDown(criteria);

            return Success(requestAssignmentsresponse.AssignmentRequests, requestAssignmentsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestAssignmentBL.GetInfo(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestAssignmentBL.GetById(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Accept(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestAssignmentBL.Accept(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Reject([FromQuery] RejectModelDTO rejectModelDTO)
        {
            if (rejectModelDTO.Id < 1)
            {
                return BadRequest();
            }
            return Success(await requestAssignmentBL.Reject(rejectModelDTO));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestAssignmentBL.Delete(requestId));
        }


    }
}