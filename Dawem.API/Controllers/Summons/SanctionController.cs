using Dawem.Contract.BusinessLogic.Summons;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Summons.Sanctions;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Summons
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class SanctionController : BaseController
    {
        private readonly ISanctionBL sanctionBL;

        public SanctionController(ISanctionBL _sanctionBL)
        {
            sanctionBL = _sanctionBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateSanctionModel model)
        {
            var result = await sanctionBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateSanctionSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateSanctionModel model)
        {

            var result = await sanctionBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateSanctionSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSanctionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var sanctionsResponse = await sanctionBL.Get(criteria);

            return Success(sanctionsResponse.Sanctions, sanctionsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetSanctionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var sanctionsResponse = await sanctionBL.GetForDropDown(criteria);

            return Success(sanctionsResponse.Sanctions, sanctionsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int sanctionId)
        {
            if (sanctionId < 1)
            {
                return BadRequest();
            }
            return Success(await sanctionBL.GetInfo(sanctionId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int sanctionId)
        {
            if (sanctionId < 1)
            {
                return BadRequest();
            }
            return Success(await sanctionBL.GetById(sanctionId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int sanctionId)
        {
            if (sanctionId < 1)
            {
                return BadRequest();
            }
            return Success(await sanctionBL.Enable(sanctionId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await sanctionBL.Disable(model));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int sanctionId)
        {
            if (sanctionId < 1)
            {
                return BadRequest();
            }
            return Success(await sanctionBL.Delete(sanctionId));
        }
        [HttpGet]
        public async Task<ActionResult> GetSanctionsInformations()
        {
            return Success(await sanctionBL.GetSanctionsInformations());
        }

    }
}