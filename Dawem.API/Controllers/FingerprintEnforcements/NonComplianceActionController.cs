using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Employees
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class NonComplianceActionController : BaseController
    {
        private readonly INonComplianceActionBL nonComplianceActionBL;

        public NonComplianceActionController(INonComplianceActionBL _nonComplianceActionBL)
        {
            nonComplianceActionBL = _nonComplianceActionBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateNonComplianceActionModel model)
        {
            var result = await nonComplianceActionBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateNonComplianceActionSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateNonComplianceActionModel model)
        {

            var result = await nonComplianceActionBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateNonComplianceActionSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetNonComplianceActionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await nonComplianceActionBL.Get(criteria);

            return Success(departmensresponse.NonComplianceActions, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetNonComplianceActionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await nonComplianceActionBL.GetForDropDown(criteria);

            return Success(departmensresponse.NonComplianceActions, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int nonComplianceActionId)
        {
            if (nonComplianceActionId < 1)
            {
                return BadRequest();
            }
            return Success(await nonComplianceActionBL.GetInfo(nonComplianceActionId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int nonComplianceActionId)
        {
            if (nonComplianceActionId < 1)
            {
                return BadRequest();
            }
            return Success(await nonComplianceActionBL.GetById(nonComplianceActionId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int nonComplianceActionId)
        {
            if (nonComplianceActionId < 1)
            {
                return BadRequest();
            }
            return Success(await nonComplianceActionBL.Delete(nonComplianceActionId));
        }
        [HttpGet]
        public async Task<ActionResult> GetNonComplianceActionsInformations()
        {
            return Success(await nonComplianceActionBL.GetNonComplianceActionsInformations());
        }

    }
}