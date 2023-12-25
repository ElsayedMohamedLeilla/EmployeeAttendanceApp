using Dawem.Contract.BusinessLogic.Core;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class EmployeeHolidayController : BaseController
    {
        private readonly IHolidayBL holidayBL;
        public EmployeeHolidayController(IHolidayBL _holidayBL)
        {
            holidayBL = _holidayBL;
        }
       
        //[HttpGet]
        //public async Task<ActionResult> Get()
        //{
           
        //    var result = await holidayBL.Get();
        //    return Success(result.Holidaies, result.TotalCount);
        //}
    }
}
