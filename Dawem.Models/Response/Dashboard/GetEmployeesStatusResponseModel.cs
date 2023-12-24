namespace Dawem.Models.Response.Employees.Employee
{
    public class GetEmployeesStatusResponseModel
    {
        public int AvailableCount { get; set; }
        public int InTaskOrAssignmentCount { get; set; }
        public int InVacationOrOutsideCount { get; set; }
    }
}
