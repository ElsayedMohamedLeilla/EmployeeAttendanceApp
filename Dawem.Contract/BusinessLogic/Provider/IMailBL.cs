using SmartBusinessERP.Models.Dtos.Shared;
using SmartBusinessERP.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IMailBL
    {


        Task<BaseResponseT<bool>> SendEmail(VerifyEmailModel sendMessageDTO);
    }
}
