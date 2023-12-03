using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateRequestTaskWithImageModel
    {
        public string UpdateRequestTaskModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
