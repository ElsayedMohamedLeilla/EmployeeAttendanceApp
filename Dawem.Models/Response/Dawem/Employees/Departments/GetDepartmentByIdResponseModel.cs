namespace Dawem.Models.Response.Dawem.Employees.Departments
{
    public class GetDepartmentByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public List<int> ManagerDelegatorIds { get; set; }
        public int? ManagerId { get; set; }
        public bool AllowFingerprintOutsideAllowedZones { get; set; }

        public List<int> ZoneIds { get; set; }
        public string Notes { get; set; }

    }
}
