using Dawem.Models.Dtos.Shared;

namespace Dawem.Contract.BusinessValidationCore.Subscriptions
{
    public interface INameTranslationBLValidationCore
    {
        Task<bool> NameTranslationsValidation(List<NameTranslationModel> model);
    }
}
