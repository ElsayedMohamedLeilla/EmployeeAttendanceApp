using Dawem.Models.Dtos.Dashboard;
using Dawem.Models.Response.Dashboard;

namespace Dawem.Contract.BusinessLogic.Dashboard
{
    public interface IDashboardBL
    {
        Task<GetHeaderInformationsResponseModel> GetHeaderInformations();
        Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations();
        Task<GetRequestsStatusResponseModel> GetRequestsStatus(GetRequestsStatusModel model);
        Task<GetEmployeesStatusResponseModel> GetEmployeesStatus();
        Task<GetDepartmentsInformationsResponseModel> GetDepartmentsInformations(GetDepartmentsInformationsCriteria model);
        Task<GetEmployeesAttendancesStatusResponseModel> GetEmployeesAttendancesStatus(GetEmployeesAttendancesStatusCriteria model);
        Task<GetBestEmployeesResponseModel> GetBestEmployees(GetBestEmployeesCriteria model);
    }
}
