namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetPermissionLogsResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public bool IsActive { get; set; }
    }
}
