using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Contract.BusinessLogic.Dawem.Provider
{
    public interface IMailBL
    {
        Task<bool> SendEmail(VerifyEmailModel sendMessageDTO);
    }
}
