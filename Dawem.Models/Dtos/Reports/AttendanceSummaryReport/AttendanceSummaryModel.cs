namespace Dawem.Models.Dtos.Reports.AttendanceSummaryReport
{
    public class AttendanceSummaryModel
    {
        public int EmployeeId { get; set; }
        public int EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public int AllWorkingDaysCount { get; set; }
        public int ActualWorkingDaysCount { get; set; }
        public int AbsencesCount { get; set; }
        public decimal WorkingHoursCount { get; set; }    
        public decimal LateArrivalsCount { get; set; }
        public decimal EarlyDeparturesCount { get; set; }
        public IEnumerable<int> Plans { get; set; }
    }
}
