namespace Dawem.Models.Response.Dawem.Employees.Departments
{
    public class GetDepartmentsForTreeResponseModel : BaseGetForDropDownResponseModel
    {
        public bool HasChildren { get; set; }
        public int ChildrenCount { get; set; }
    }
}
