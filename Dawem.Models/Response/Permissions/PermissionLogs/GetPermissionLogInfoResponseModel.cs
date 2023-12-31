namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetPermissionLogInfoResponseModel
    {
        public int Code { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public string ActionName { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
