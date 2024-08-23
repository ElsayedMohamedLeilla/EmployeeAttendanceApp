namespace Dawem.Models.Response.Dawem.ReportCritrias.BaseData
{
    public class SummonsReportCritria : BaseReportCritria
    {
        public List<int> EmployeesIds { get; set; }
        public List<int> GroupsIds { get; set; }
        public List<int> DepartmentsIds { get; set; }
    }
}
