using Dawem.Contract.BusinessLogicCore;
using Dawem.Helpers;
using Dawem.Models.Dtos.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Drawing;

namespace Dawem.BusinessLogicCore
{
    public class UploadBLC : IUploadBLC
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly LinkGenerator generator;
        private readonly IHttpContextAccessor accessor;
        public UploadBLC(LinkGenerator _generator,
            IHttpContextAccessor _accessor,
            IWebHostEnvironment _webHostEnvironment)
        {
            generator = _generator;
            accessor = _accessor;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<UploadResult> UploadFile(IFormFile file, string FolderName)
        {
            UploadResult uploadResult;
            try
            {
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var uploadsDirectory = Path.Combine(webHostEnvironment.WebRootPath, LeillaKeys.Uploads, FolderName);
                var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }
                if (Directory.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    if (file.ContentType.Contains(LeillaKeys.Image) && file.Length > 300000)
                    {
                        await file.CopyToAsync(stream);
                        var img = Image.FromStream(stream);
                        var resizedimg = ImageHelper.ResizeImage(img, 300, 300);
                        resizedimg.Save(uniqueFileName);
                    }
                    else
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                uploadResult = new()
                {
                    FileName = uniqueFileName,
                    FolderName = FolderName,
                    Path = GetFilePath(uniqueFileName, FolderName)
                };

            }
            catch (Exception ex)
            {
                return null;
            }

            return uploadResult;
        }
        public string GetFilePath(string fileName, string folderName)
        {
            var protocol = (accessor?.HttpContext?.Request?.IsHttps ?? true) ? LeillaKeys.Https : LeillaKeys.Http;
            var host = accessor?.HttpContext?.Request?.Host.Value;
            var path = generator.GetPathByAction(LeillaKeys.Browse, LeillaKeys.Browse, null);
            var browseLink = $"{protocol}://{host}{path}";

            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrEmpty(fileName)) return null;
            return browseLink + LeillaKeys.QuestionMark + "fileName=" + fileName + "&folderName=" + folderName;
        }
        private static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return Path.GetFileNameWithoutExtension(fileName)
                      + LeillaKeys.UnderScore
                      + Guid.NewGuid().ToString()
                      + LeillaKeys.UnderScore
                      + DateTime.Now.ToString("yyyyMMddHHmmssfff")
                      + Path.GetExtension(fileName);
        }
    }
}

