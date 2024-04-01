namespace Dawem.Models.Response.Dawem.Employees.Employees
{
    public class GetEmployeesSchedulePlanResponse
    {
        public List<GetCurrentEmployeeScheduleInPeriodDTO> EmployeeSchedulePlan { get; set; }
        public int TotalCount { get; set; }
    }
}
