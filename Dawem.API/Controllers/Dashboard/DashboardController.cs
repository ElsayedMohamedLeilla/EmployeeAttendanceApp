using Dawem.Contract.BusinessLogic.Dashboard;
using Dawem.Models.Dtos.Dashboard;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Dashboard
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
        public async Task<ActionResult> GetHeaderInformations()
        {
            var response = await dashboardBL.GetHeaderInformations();
            return Success(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeesAttendancesInformations()
        {
            var response = await dashboardBL.GetEmployeesAttendancesInformations();
            return Success(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetRequestsStatus([FromQuery] GetRequestsStatusModel model)
        {
            var response = await dashboardBL.GetRequestsStatus(model);
            return Success(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeesStatus()
        {
            var response = await dashboardBL.GetEmployeesStatus();
            return Success(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetDepartmentsInformations([FromQuery] GetDepartmentsInformationsCriteria model)
        {
            var departmentsResponse = await dashboardBL.GetDepartmentsInformations(model);
            return Success(departmentsResponse.Departments, departmentsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployeesAttendancesStatus([FromQuery] GetEmployeesAttendancesStatusCriteria model)
        {
            var employeesResponse = await dashboardBL.GetEmployeesAttendancesStatus(model);
            return Success(employeesResponse.Employees, employeesResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetBestEmployees([FromQuery] GetBestEmployeesCriteria model)
        {
            var employeesResponse = await dashboardBL.GetBestEmployees(model);
            return Success(employeesResponse.Employees, employeesResponse.TotalCount);
        }
    }
}