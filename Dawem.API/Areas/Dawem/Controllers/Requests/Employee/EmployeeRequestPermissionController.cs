using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Permissions;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.Requests.Employee
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class EmployeeRequestPermissionController : BaseController
    {
        private readonly IRequestPermissionBL requestPermissionBL;


        public EmployeeRequestPermissionController(IRequestPermissionBL _requestPermissionBL)
        {
            requestPermissionBL = _requestPermissionBL;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Create([FromForm] CreateRequestPermissionWithImageModel formData)
        {
            if (formData == null || formData.CreateRequestPermissionModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<CreateRequestPermissionModelDTO>(formData.CreateRequestPermissionModelString);
            model.Attachments = formData.Attachments;
            var result = await requestPermissionBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreatePermissionRequestSuccessfully);
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<ActionResult> Update([FromForm] UpdateRequestPermissionWithImageModel formData)
        {
            if (formData == null || formData.UpdateRequestPermissionModelString == null)
            {
                return BadRequest();
            }

            var model = JsonConvert.DeserializeObject<UpdateRequestPermissionModelDTO>(formData.UpdateRequestPermissionModelString);
            model.Attachments = formData.Attachments;
            var result = await requestPermissionBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdatePermissionRequestSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] EmployeeGetRequestPermissionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestPermissionsresponse = await requestPermissionBL.EmployeeGet(criteria);

            return Success(requestPermissionsresponse.PermissionRequests, requestPermissionsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetRequestPermissionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var requestPermissionsresponse = await requestPermissionBL.GetForDropDown(criteria);

            return Success(requestPermissionsresponse.PermissionRequests, requestPermissionsresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestPermissionBL.GetInfo(requestId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestPermissionBL.GetById(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Accept(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestPermissionBL.Accept(requestId));
        }
        [HttpPut]
        public async Task<ActionResult> Reject([FromQuery] RejectModelDTO rejectModelDTO)
        {
            if (rejectModelDTO.Id < 1)
            {
                return BadRequest();
            }
            return Success(await requestPermissionBL.Reject(rejectModelDTO));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int requestId)
        {
            if (requestId < 1)
            {
                return BadRequest();
            }
            return Success(await requestPermissionBL.Delete(requestId));
        }


    }
}