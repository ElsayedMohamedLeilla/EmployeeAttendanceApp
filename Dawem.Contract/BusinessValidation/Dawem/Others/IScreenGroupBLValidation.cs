using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;

namespace Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions
{
    public interface IScreenGroupBLValidation
    {
        Task<bool> CreateValidation(CreateScreenGroupModel model);
        Task<bool> UpdateValidation(UpdateScreenGroupModel model);
    }
}
