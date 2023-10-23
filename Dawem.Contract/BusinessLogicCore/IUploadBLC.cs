using Dawem.Models.Dtos.Others;
using Microsoft.AspNetCore.Http;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IUploadBLC
    {
        Task<UploadResult> UploadImageFile(IFormFile imageFile, string FolderName);
        string GetFilePath(string fileName, string folderName);
    }
}
