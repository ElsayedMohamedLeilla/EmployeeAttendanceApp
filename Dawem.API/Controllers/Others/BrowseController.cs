using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Others
{
    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    public class BrowseController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public BrowseController(IWebHostEnvironment _webHostEnvironment)
        {
            webHostEnvironment = _webHostEnvironment;
        }
        [HttpGet]

        public ActionResult Browse(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
                return BadRequest();

            string imgPath;
            if (!string.IsNullOrEmpty(folderName))
                imgPath = Path.Combine(webHostEnvironment.WebRootPath, DawemKeys.Uploads, folderName, fileName);

            else
                imgPath = Path.Combine(webHostEnvironment.WebRootPath, DawemKeys.Uploads, fileName);
            if (System.IO.File.Exists(imgPath))
            {
                var image = System.IO.File.OpenRead(imgPath);
                return File(image, "image/jpeg");
            }


            return BadRequest();

        }
    }
}