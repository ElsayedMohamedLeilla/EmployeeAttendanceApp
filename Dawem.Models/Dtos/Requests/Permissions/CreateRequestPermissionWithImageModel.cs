using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Permissions
{
    public class CreateRequestPermissionWithImageModel
    {
        public string CreateRequestPermissionModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
