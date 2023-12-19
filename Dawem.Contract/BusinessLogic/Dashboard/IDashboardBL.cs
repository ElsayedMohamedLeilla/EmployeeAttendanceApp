using Dawem.Models.Response.Employees.Employee;

namespace Dawem.Contract.BusinessLogic.Employees
{
    public interface IDashboardBL
    {
        Task<GetEmployeeInfoResponseModel> GetInfo(int employeeId);
        Task<GetEmployeesAttendancesInformationsResponseModel> GetEmployeesAttendancesInformations();
    }
}
