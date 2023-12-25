using Dawem.BusinessLogic.Core.Zones;
using Dawem.BusinessLogic.Employees.Departments;
using Dawem.Contract.BusinessLogic.Attendances;
using Dawem.Models.Dtos.Attendances.FingerprintDevices;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Attendances
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class FingerprintDeviceController : BaseController
    {
        private readonly IFingerprintDeviceBL fingerprintDeviceBL;

        public FingerprintDeviceController(IFingerprintDeviceBL _fingerprintDeviceBL)
        {
            fingerprintDeviceBL = _fingerprintDeviceBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateFingerprintDeviceModel model)
        {
            var result = await fingerprintDeviceBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateFingerprintDeviceSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateFingerprintDeviceModel model)
        {

            var result = await fingerprintDeviceBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateFingerprintDeviceSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetFingerprintDevicesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await fingerprintDeviceBL.Get(criteria);

            return Success(departmensresponse.FingerprintDevices, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetFingerprintDevicesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await fingerprintDeviceBL.GetForDropDown(criteria);

            return Success(departmensresponse.FingerprintDevices, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int fingerprintDeviceId)
        {
            if (fingerprintDeviceId < 1)
            {
                return BadRequest();
            }
            return Success(await fingerprintDeviceBL.GetInfo(fingerprintDeviceId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int fingerprintDeviceId)
        {
            if (fingerprintDeviceId < 1)
            {
                return BadRequest();
            }
            return Success(await fingerprintDeviceBL.GetById(fingerprintDeviceId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int fingerprintDeviceId)
        {
            if (fingerprintDeviceId < 1)
            {
                return BadRequest();
            }
            return Success(await fingerprintDeviceBL.Enable(fingerprintDeviceId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await fingerprintDeviceBL.Disable(model));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int fingerprintDeviceId)
        {
            if (fingerprintDeviceId < 1)
            {
                return BadRequest();
            }
            return Success(await fingerprintDeviceBL.Delete(fingerprintDeviceId));
        }
        [HttpGet]
        public async Task<ActionResult> GetFingerprintDevicesInformations()
        {
            return Success(await fingerprintDeviceBL.GetFingerprintDevicesInformations());
        }
    }
}