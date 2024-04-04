using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Others
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController]
   
    public class BrowseController : DawemControllerBase
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
                imgPath = Path.Combine(webHostEnvironment.WebRootPath, LeillaKeys.Uploads, folderName, fileName);

            else
                imgPath = Path.Combine(webHostEnvironment.WebRootPath, LeillaKeys.Uploads, fileName);
            if (System.IO.File.Exists(imgPath))
            {
                var svgContentType = "image/svg+xml; charset=utf-8";
                var pdfContentType = "application/pdf";
                var imageContentType = "image/jpeg";
                var sheetContentType = "application/vnd.ms-excel";
                var wordContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                var txtContentType = "text/plain";

                var contentType = fileName.Contains(".svg") ? svgContentType :
                    fileName.Contains(".pdf") ? pdfContentType :
                    fileName.Contains(".docx") ? wordContentType :
                    fileName.Contains(".txt") ? txtContentType :
                    fileName.Contains(".xlsx") || fileName.Contains(".xls") || fileName.Contains(".csv") ? sheetContentType :
                    imageContentType;

                var imageFile = System.IO.File.OpenRead(imgPath);
                return File(imageFile, contentType, fileName);
            }

            return BadRequest();

        }
    }
}