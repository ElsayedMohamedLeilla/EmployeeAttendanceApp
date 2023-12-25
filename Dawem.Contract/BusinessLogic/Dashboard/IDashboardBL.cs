using Dawem.Models.Dtos.Employees.Department;
using Dawem.Models.Response.Employees.Employee;

namespace Dawem.Contract.BusinessLogic.Employees
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
