namespace Dawem.Models.Response.Dawem.ReportCritrias.BaseData
{
    public class RequestsReportCritria : BaseReportCritria
    {
        public List<int> EmployeeIds { get; set; }
        public List<int> GroupIds { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}
