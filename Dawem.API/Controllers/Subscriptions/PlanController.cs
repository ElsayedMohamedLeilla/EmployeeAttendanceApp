using Dawem.Contract.BusinessLogic.Subscriptions;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Subscriptions.Plans;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Subscriptions
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class PlanController : BaseController
    {
        private readonly IPlanBL planBL;

        public PlanController(IPlanBL _planBL)
        {
            planBL = _planBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePlanModel model)
        {
            var result = await planBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreatePlanSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdatePlanModel model)
        {

            var result = await planBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdatePlanSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetPlansCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await planBL.Get(criteria);

            return Success(departmensresponse.Plans, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetPlansCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await planBL.GetForDropDown(criteria);

            return Success(departmensresponse.Plans, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int planId)
        {
            if (planId < 1)
            {
                return BadRequest();
            }
            return Success(await planBL.GetInfo(planId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int planId)
        {
            if (planId < 1)
            {
                return BadRequest();
            }
            return Success(await planBL.GetById(planId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int planId)
        {
            if (planId < 1)
            {
                return BadRequest();
            }
            return Success(await planBL.Delete(planId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int planId)
        {
            if (planId < 1)
            {
                return BadRequest();
            }
            return Success(await planBL.Enable(planId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await planBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetPlansInformations()
        {
            return Success(await planBL.GetPlansInformations());
        }
    }
}