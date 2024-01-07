using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Employees
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class FingerprintEnforcementController : BaseController
    {
        private readonly IFingerprintEnforcementBL assignmentTypeBL;

        public FingerprintEnforcementController(IFingerprintEnforcementBL _assignmentTypeBL)
        {
            assignmentTypeBL = _assignmentTypeBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateFingerprintEnforcementModel model)
        {
            var result = await assignmentTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateFingerprintEnforcementSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateFingerprintEnforcementModel model)
        {

            var result = await assignmentTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateFingerprintEnforcementSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetFingerprintEnforcementsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await assignmentTypeBL.Get(criteria);

            return Success(departmensresponse.FingerprintEnforcements, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int assignmentTypeId)
        {
            if (assignmentTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await assignmentTypeBL.GetInfo(assignmentTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int assignmentTypeId)
        {
            if (assignmentTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await assignmentTypeBL.GetById(assignmentTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int assignmentTypeId)
        {
            if (assignmentTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await assignmentTypeBL.Delete(assignmentTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetFingerprintEnforcementsInformations()
        {
            return Success(await assignmentTypeBL.GetFingerprintEnforcementsInformations());
        }

    }
}