using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.VacationsTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class VacationsTypeController : BaseController
    {
        private readonly IVacationsTypeBL VacationsTypeBL;
        public VacationsTypeController(IVacationsTypeBL _VacationsTypeBL)
        {
            VacationsTypeBL = _VacationsTypeBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateVacationsTypeDTO model)
        {
            var result = await VacationsTypeBL.Create(model);
            return Success(result, messageCode: DawemKeys.DoneCreateVacationsTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateVacationsTypeDTO model)
        {

            var result = await VacationsTypeBL.Update(model);
            return Success(result, messageCode: DawemKeys.DoneUpdateVacationsTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetVacationsTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await VacationsTypeBL.Get(criteria);
            return Success(result.VacationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetVacationsTypeCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await VacationsTypeBL.GetForDropDown(criteria);
            return Success(result.VacationsTypes, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int VacationsTypeId)
        {
            if (VacationsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await VacationsTypeBL.GetInfo(VacationsTypeId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int VacationsTypeId)
        {
            if (VacationsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await VacationsTypeBL.GetById(VacationsTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int VacationsTypeId)
        {
            if (VacationsTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await VacationsTypeBL.Delete(VacationsTypeId));
        }

    }
}
