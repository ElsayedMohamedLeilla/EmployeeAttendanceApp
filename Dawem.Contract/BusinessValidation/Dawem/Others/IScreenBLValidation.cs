using Dawem.Models.DTOs.Dawem.Screens.Screens;

namespace Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions
{
    public interface IScreenBLValidation
    {
        Task<bool> CreateValidation(CreateScreenModel model);
        Task<bool> UpdateValidation(UpdateScreenModel model);
    }
}
