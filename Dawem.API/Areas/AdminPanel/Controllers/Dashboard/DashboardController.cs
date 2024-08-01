using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.Dawem.Dashboard;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Dashboard
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DashboardController : AdminPanelControllerBase
    {
        private readonly IDashboardBL dashboardBL;
        public DashboardController(IDashboardBL _dashboardBL)
        {
            dashboardBL = _dashboardBL;
        }

        [HttpGet]
        public async Task<ActionResult> GetHeaderInformations()
        {
            var response = await dashboardBL.GetHeaderInformationsForAdminPanel();
            return Success(response);
        }
    }
}