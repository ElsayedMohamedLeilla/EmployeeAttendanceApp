using Dawem.Models.Dtos.Dawem.Others;
using Microsoft.AspNetCore.Http;

namespace Dawem.Contract.BusinessLogicCore.Dawem
{
    public interface IUploadBLC
    {
        Task<UploadResult> UploadFile(IFormFile imageFile, string FolderName);
        string GetFilePath(string fileName, string folderName);
    }
}
