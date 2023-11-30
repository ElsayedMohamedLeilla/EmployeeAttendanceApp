using Dawem.Models.Response.Employees.TaskTypes;

namespace Dawem.Models.Response.Employees.Departments
{
    public class GetDepartmentsForTreeResponseModel : BaseGetForDropDownResponseModel
    {
        public bool HasChildren { get; set; }
    }
}
