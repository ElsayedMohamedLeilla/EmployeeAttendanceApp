using Dawem.Models.Dtos.Shared;
using Dawem.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IMailBL
    {


        Task<BaseResponseT<bool>> SendEmail(VerifyEmailModel sendMessageDTO);
    }
}
