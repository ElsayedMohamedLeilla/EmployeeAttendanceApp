using Dawem.Models.Dtos.Dawem.Core.JustificationsTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IJustificationTypeBLValidation
    {
        Task<bool> CreateValidation(CreateJustificationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateJustificationsTypeDTO model);
    }
}
