
using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Provider
{
    public class UpdateEmployeeModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public DateTime JoiningDate { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public bool IsActive { get; set; }
    }
}
