using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Tasks;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Requests.Employee
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class EmployeeRequestTaskController : DawemControllerBase
    {
        private readonly IRequestTaskBL requestTaskBL;


        public EmployeeRequestTaskController(IRequestTaskBL _requestTaskBL)
        {
            requestTaskBL = _requestTaskBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateRequestTaskWithImageModel formData)
        {
            if (formData == null || formData.CreateRequestTaskModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateRequestTaskModelDTO>(formData.CreateRequestTaskModelString);
            model.Attachments = formData.Attachments;
            var result = await requestTaskBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateTaskRequestSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateRequestTaskWithImageModel formData)
        {
            if (formData == null || formData.UpdateRequestTaskModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateRequestTaskModelDTO>(formData.UpdateRequestTaskModelString);
            model.Attachments = formData.Attachments;
            var result = await requestTaskBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateTaskRequestSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] EmployeeGetRequestTasksCriteria model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var response = await requestTaskBL.EmployeeGet(model);
            return Success(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetRequestTasksCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestTasksresponse = await requestTaskBL.GetForDropDown(criteria);

            return Success(requestTasksresponse.TaskRequests, requestTasksresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestTaskBL.GetInfo(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestTaskBL.GetById(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Accept(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestTaskBL.Accept(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Reject([FromQuery] RejectModelDTO rejectModelDTO)
        {
            if (rejectModelDTO.Id < 1)
            {
                return BadRequest();
            }
            return Success(await requestTaskBL.Reject(rejectModelDTO));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestTaskBL.Delete(requestId));
        }


    }
}