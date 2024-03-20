namespace Dawem.Models.Dtos.Excel.Departments
{
    public class DepartmentHeaderDraftDTO
    {
        public string DepartmentName { get; set; }
        public string ParentDepartment { get; set; }
        public string ManagerName { get; set; }
        public bool IsActive { get; set; }
    }
}
