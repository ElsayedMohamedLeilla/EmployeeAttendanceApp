using Dawem.Models.Dtos.Others;
using Microsoft.AspNetCore.Http;

namespace Dawem.Contract.BusinessLogicCore
{
    public interface IUploadBLC
    {
        Task<UploadResult> UploadFile(IFormFile imageFile, string FolderName);
        string GetFilePath(string fileName, string folderName);
    }
}
