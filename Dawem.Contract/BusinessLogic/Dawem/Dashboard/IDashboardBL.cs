using Dawem.Models.Dtos.Dawem.Dashboard;
using Dawem.Models.Response.Dashboard;

namespace Dawem.Contract.BusinessLogic.Dawem.Dashboard
{
    public interface IDashboardBL
    {
        Task<GetHeaderInformationsResponseModel> GetHeaderInformations();
        Task<EmployeeGetHeaderInformationsResponseModel> EmployeeGetHeaderInformations();
        Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations();
        Task<GetRequestsStatusResponseModel> GetRequestsStatus(GetRequestsStatusModel model);
        Task<GetEmployeesStatusResponseModel> GetEmployeesStatus();
        Task<GetDepartmentsInformationsResponseModel> GetDepartmentsInformations(GetDepartmentsInformationsCriteria model);
        Task<GetEmployeesAttendancesStatusResponseModel> GetEmployeesAttendancesStatus(GetEmployeesAttendancesStatusCriteria model);
        Task<GetBestEmployeesResponseModel> GetBestEmployees(GetBestEmployeesCriteria model);
    }
}
