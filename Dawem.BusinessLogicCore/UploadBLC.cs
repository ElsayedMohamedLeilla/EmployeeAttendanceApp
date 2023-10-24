using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Dtos.Others;
using Dawem.Translations;
using Glamatek.Utils.Helpers;
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

        public async Task<UploadResult> UploadImageFile(IFormFile imageFile, string FolderName)
        {
            UploadResult uploadResult;
            try
            {
                //TO DO : check file extension

                var uniqueFileName = GetUniqueFileName(imageFile.FileName);
                var uploadsDirectory = Path.Combine(webHostEnvironment.WebRootPath, DawemKeys.Uploads, FolderName);
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
                    if (imageFile.Length > 300000)
                    {
                        await imageFile.CopyToAsync(stream);
                        var img = Image.FromStream(stream);
                        var resizedimg = ImageHelper.ResizeImage(img, 300, 300);
                        resizedimg.Save(uniqueFileName);
                    }
                    else
                    {
                        await imageFile.CopyToAsync(stream);
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
            var protocol = (accessor?.HttpContext?.Request?.IsHttps ?? true) ? DawemKeys.Https : DawemKeys.Http;
            var host = accessor?.HttpContext?.Request?.Host.Value;
            var path = generator.GetPathByAction(DawemKeys.Browse, DawemKeys.Browse, null);
            var browseLink = $"{protocol}://{host}{path}";

            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrEmpty(fileName)) return DawemKeys.EmptyString;
            return browseLink + DawemKeys.QuestionMark + "fileName=" + fileName + "&folderName=" + folderName;
        }
        private static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return Path.GetFileNameWithoutExtension(fileName)
                      + DawemKeys.UnderScore
                      + Guid.NewGuid().ToString()
                      + DawemKeys.UnderScore
                      + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff")
                      + Path.GetExtension(fileName);
        }
    }
}

