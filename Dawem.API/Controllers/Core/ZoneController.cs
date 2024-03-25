using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Zones;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class ZoneController : BaseController
    {
        private readonly IZoneBL zoneBL;
        public ZoneController(IZoneBL _ZoneBL)
        {
            zoneBL = _ZoneBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateZoneDTO model)
        {
            var result = await zoneBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreateZoneSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateZoneDTO model)
        {

            var result = await zoneBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdateZoneSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetZoneCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await zoneBL.Get(criteria);
            return Success(result.Zones, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetZoneCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await zoneBL.GetForDropDown(criteria);
            return Success(result.Zones, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await zoneBL.GetInfo(ZoneId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await zoneBL.GetById(ZoneId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await zoneBL.Delete(ZoneId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int ZoneId)
        {
            if (ZoneId < 1)
            {
                return BadRequest();
            }
            return Success(await zoneBL.Enable(ZoneId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await zoneBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetZonesInformations()
        {
            return Success(await zoneBL.GetZonesInformations());
        }

        [HttpGet]
        public async Task<ActionResult> CreateExportDraft()
        {
            var stream = await zoneBL.ExportDraft();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", AmgadKeys.ZoneEmptyDraft);
        }

        [HttpPost]
        [RequestSizeLimit(10 * 2048 * 2048)]
        public async Task<IActionResult> CreateImportDataFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(AmgadKeys.NoFileUploaded);
            }

            using Stream fileStream = file.OpenReadStream();
            Dictionary<string, string> result = await zoneBL.ImportDataFromExcelToDB(fileStream);

            if (result.ContainsKey(AmgadKeys.Success))
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
    }
}
