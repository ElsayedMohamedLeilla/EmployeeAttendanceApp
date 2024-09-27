using Dawem.Models.DTOs.Dawem.Screens.Screens;

namespace Dawem.Contract.BusinessValidation.Dawem.Others
{
    public interface IScreenBLValidation
    {
        Task<bool> CreateValidation(CreateScreenModel model);
        Task<bool> UpdateValidation(UpdateScreenModel model);
    }
}
