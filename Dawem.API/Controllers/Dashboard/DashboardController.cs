using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Dashboards
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardBL dashboardBL;


        public DashboardController(IDashboardBL _dashboardBL)
        {
            dashboardBL = _dashboardBL;
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeesAttendancesInformations()
        {
            var response = await dashboardBL.GetEmployeesAttendancesInformations();
            return Success(response);
        }



    }
}