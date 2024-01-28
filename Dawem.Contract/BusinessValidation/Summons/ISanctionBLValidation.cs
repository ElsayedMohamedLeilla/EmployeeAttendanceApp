using Dawem.Models.Dtos.Summons.Sanctions;

namespace Dawem.Contract.BusinessValidation.Summons
{
    public interface ISanctionBLValidation
    {
        Task<bool> CreateValidation(CreateSanctionModel model);
        Task<bool> UpdateValidation(UpdateSanctionModel model);
    }
}
