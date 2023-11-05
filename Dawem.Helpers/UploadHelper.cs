using Dawem.Models.Dtos.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace Dawem.Helpers
{
    public static class UploadHelper
    {
        public static async Task<UploadResult> UploadImageFile(IFormFile imageFile, string FolderName, IWebHostEnvironment webHostEnvironment)
        {
            UploadResult uploadResult = null;
            try
            {
                var uniqueFileName = GetUniqueFileName(imageFile.FileName);
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

                    if (stream.Length > 300000)
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
                    //Path = GetFilePath(uniqueFileName, FolderName, webHostEnvironment)
                };

            }
            catch (Exception ex)
            {
                return null;
            }

            return uploadResult;
        }
        /*public static string GetFilePath(string fileName, string fileFolder)
        {
            return Path.Combine(webHostEnvironment.WebRootPath, DawemKeys.Uploads, fileFolder, fileName);

            var path = generator.GetPathByAction(DawemKeys.VerifyEmail, DawemKeys.Account, emailToken);
            var protocol = accessor.HttpContext.Request.IsHttps ? DawemKeys.Https : DawemKeys.Http;
            var host = accessor.HttpContext.Request.Host.Value;
            var confirmEmailLink = $"{protocol}://{host}{path}";
            return confirmEmailLink;

            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrEmpty(fileName)) return null;
            var baseUrl = webHostEnvironment.WebRootPath + "api/Upload/Images?";
            return baseUrl + "na=" + fileName + "&fld=" + fileFolder;
        }*/
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
