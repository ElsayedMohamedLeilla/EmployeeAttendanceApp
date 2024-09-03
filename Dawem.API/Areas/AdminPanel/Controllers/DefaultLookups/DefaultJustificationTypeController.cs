using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.DefaultLookups
{

    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    public class DefaultJustificationTypeController : AdminPanelControllerBase
    {
        private readonly IDefaultJustificationTypeBL JustificationTypeBL;
        public DefaultJustificationTypeController(IDefaultJustificationTypeBL _JustificationTypeBL)
        {
            JustificationTypeBL = _JustificationTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateDefaultJustificationsTypeDTO model)
        {
            var result = await JustificationTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateJustificationsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateDefaultJustificationsTypeDTO model)
        {

            var result = await JustificationTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateJustificationsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetDefaultJustificationTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await JustificationTypeBL.Get(criteria);
            return Success(result.DefaultJustificationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetDefaultJustificationTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await JustificationTypeBL.GetForDropDown(criteria);
            return Success(result.DefaultJustificationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int JustificationTypeId)
        {
            if (JustificationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await JustificationTypeBL.GetInfo(JustificationTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int JustificationTypeId)
        {
            if (JustificationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await JustificationTypeBL.GetById(JustificationTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int JustificationTypeId)
        {
            if (JustificationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await JustificationTypeBL.Delete(JustificationTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int JustificationTypeId)
        {
            if (JustificationTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await JustificationTypeBL.Enable(JustificationTypeId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await JustificationTypeBL.Disable(model));
        }

    }
}
