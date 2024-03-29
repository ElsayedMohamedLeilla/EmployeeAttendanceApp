using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions
{
    public interface INameTranslationBLValidationCore
    {
        Task<bool> NameTranslationsValidation(List<NameTranslationModel> model);
    }
}
