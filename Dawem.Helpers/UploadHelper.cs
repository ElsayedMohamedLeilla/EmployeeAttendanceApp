using Dawem.Models.Dtos.Others;
using Dawem.Translations;
using Glamatek.Utils.Helpers;
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
                    Path = GetFilePath(uniqueFileName, FolderName, webHostEnvironment)
                };

            }
            catch (Exception ex)
            {
                return null;
            }

            return uploadResult;
        }
        public static string GetFilePath(string fileName, string fileFolder, IWebHostEnvironment webHostEnvironment)
        {
            return Path.Combine(webHostEnvironment.WebRootPath, DawemKeys.Uploads, fileFolder, fileName);
            
            /*if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrEmpty(fileName)) return null;
            var baseUrl = webHostEnvironment.WebRootPath + "api/Upload/Images?";
            return baseUrl + "na=" + fileName + "&fld=" + fileFolder;*/
        }
        private static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return Path.GetFileNameWithoutExtension(fileName)
                      + DawemKeys.UnderScore
                      + Guid.NewGuid().ToString()
                      + DawemKeys.UnderScore
                      + DateTime.Now.ToString("yyyyMMddHHmmssfff")
                      + Path.GetExtension(fileName);
        }
    }
}
