using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultPenaltiesController : AdminPanelControllerBase
    {
        private readonly IDefaultPenaltiesBL PenaltiesBL;
        public DefaultPenaltiesController(IDefaultPenaltiesBL _PenaltiesBL)
        {
            PenaltiesBL = _PenaltiesBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultPenaltiesDTO model)
        {
            var result = await PenaltiesBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreatePenaltySuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultPenaltiesDTO model)
        {

            var result = await PenaltiesBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdatePenaltySuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultPenaltiesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await PenaltiesBL.Get(criteria);
            return Success(result.DefaultPenalties, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultPenaltiesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await PenaltiesBL.GetForDropDown(criteria);
            return Success(result.DefaultPenalties, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int PenaltiesId)
        {
            if (PenaltiesId < 1)
            {
                return BadRequest();
            }
            return Success(await PenaltiesBL.GetInfo(PenaltiesId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int PenaltiesId)
        {
            if (PenaltiesId < 1)
            {
                return BadRequest();
            }
            return Success(await PenaltiesBL.GetById(PenaltiesId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int PenaltiesId)
        {
            if (PenaltiesId < 1)
            {
                return BadRequest();
            }
            return Success(await PenaltiesBL.Delete(PenaltiesId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int PenaltiesId)
        {
            if (PenaltiesId < 1)
            {
                return BadRequest();
            }
            return Success(await PenaltiesBL.Enable(PenaltiesId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await PenaltiesBL.Disable(model));
        }

    }
}
