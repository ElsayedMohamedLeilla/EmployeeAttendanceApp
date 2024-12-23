using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]


    public class OvertimeTypeController : DawemControllerBase
    {
        private readonly IOvertimeTypeBL overtimeTypeBL;
        public OvertimeTypeController(IOvertimeTypeBL _overtimeTypeBL)
        {
            overtimeTypeBL = _overtimeTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateOvertimeTypeDTO model)
        {
            var result = await overtimeTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateOvertimeTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateOvertimeTypeDTO model)
        {

            var result = await overtimeTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateOvertimeTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetOvertimeTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await overtimeTypeBL.Get(criteria);
            return Success(result.OvertimesTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetOvertimeTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await overtimeTypeBL.GetForDropDown(criteria);
            return Success(result.OvertimesTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int overtimeTypeId)
        {
            if (overtimeTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await overtimeTypeBL.GetInfo(overtimeTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int overtimeTypeId)
        {
            if (overtimeTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await overtimeTypeBL.GetById(overtimeTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int overtimeTypeId)
        {
            if (overtimeTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await overtimeTypeBL.Delete(overtimeTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetOvertimeTypesInformations()
        {
            return Success(await overtimeTypeBL.GetOvertimeTypesInformations());
        }
    }
}
