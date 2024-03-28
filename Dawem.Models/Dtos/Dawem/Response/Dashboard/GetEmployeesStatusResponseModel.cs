namespace Dawem.Models.Response.Dashboard
{
    public class GetEmployeesStatusResponseModel
    {
        public int AvailableCount { get; set; }
        public int InTaskOrAssignmentCount { get; set; }
        public int InVacationOrOutsideCount { get; set; }
    }
}
