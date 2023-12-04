using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Requests.Permissions
{
    public class UpdateRequestPermissionWithImageModel
    {
        public string UpdateRequestPermissionModelString { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
