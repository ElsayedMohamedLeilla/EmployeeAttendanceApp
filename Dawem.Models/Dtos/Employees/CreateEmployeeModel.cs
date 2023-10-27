using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees
{
    public class CreateEmployeeModel
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public DateTime JoiningDate { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public bool IsActive { get; set; }

    }
}
