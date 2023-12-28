using Dawem.Contract.BusinessLogic.Dashboard;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Dashboard
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class EmployeeDashboardController : BaseController
    {
        private readonly IDashboardBL dashboardBL;
        public EmployeeDashboardController(IDashboardBL _dashboardBL)
        {
            dashboardBL = _dashboardBL;
        }

        [HttpGet]
        public async Task<ActionResult> GetHeaderInformations()
        {
            var response = await dashboardBL.EmployeeGetHeaderInformations();
            return Success(response);
        }
    }
}