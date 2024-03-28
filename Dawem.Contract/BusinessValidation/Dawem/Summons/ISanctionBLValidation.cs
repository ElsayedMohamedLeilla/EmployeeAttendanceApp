using Dawem.Models.Dtos.Dawem.Summons.Sanctions;

namespace Dawem.Contract.BusinessValidation.Dawem.Summons
{
    public interface ISanctionBLValidation
    {
        Task<bool> CreateValidation(CreateSanctionModel model);
        Task<bool> UpdateValidation(UpdateSanctionModel model);
    }
}
