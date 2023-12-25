using Dawem.Models.Dtos.Core.JustificationsTypes;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IJustificationTypeBLValidation
    {
        Task<bool> CreateValidation(CreateJustificationsTypeDTO model);
        Task<bool> UpdateValidation(UpdateJustificationsTypeDTO model);
    }
}
