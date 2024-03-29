using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Dawem.Employees.Employees
{
    public class UpdateSpecificModelDTO
    {
      //  public int Id { get; set; }
        public string Address { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public string ProfileImageName { get; set; }


    }
}