using Dawem.Models.Dtos.Shared;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IMailBL
    {
        Task<bool> SendEmail(VerifyEmailModel sendMessageDTO);
    }
}
