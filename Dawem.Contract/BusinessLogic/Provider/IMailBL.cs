using Dawem.Models.Dtos.Shared;
using Dawem.Models.Response;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IMailBL
    {


        Task<BaseResponseT<bool>> SendEmail(VerifyEmailModel sendMessageDTO);
    }
}
