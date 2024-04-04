using Dawem.Contract.BusinessLogic.Dawem.Dashboard;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Dashboard
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class EmployeeDashboardController : DawemControllerBase
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