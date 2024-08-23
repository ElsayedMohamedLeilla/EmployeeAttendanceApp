using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;

namespace Dawem.Contract.BusinessValidation.Dawem.Others
{
    public interface IScreenGroupBLValidation
    {
        Task<bool> CreateValidation(CreateScreenGroupModel model);
        Task<bool> UpdateValidation(UpdateScreenGroupModel model);
    }
}
