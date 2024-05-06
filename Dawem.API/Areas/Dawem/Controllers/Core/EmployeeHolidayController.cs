using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    
    
    public class EmployeeHolidayController : DawemControllerBase
    {
        private readonly IHolidayBL holidayBL;
        public EmployeeHolidayController(IHolidayBL _holidayBL)
        {
            holidayBL = _holidayBL;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await holidayBL.GetForEmployee();
            return Success(result);
        }
    }
}
