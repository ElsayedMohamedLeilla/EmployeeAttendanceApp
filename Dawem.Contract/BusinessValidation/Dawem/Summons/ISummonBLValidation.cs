using Dawem.Models.Dtos.Dawem.Summons.Summons;

namespace Dawem.Contract.BusinessValidation.Dawem.Summons
{
    public interface ISummonBLValidation
    {
        Task<bool> CreateValidation(CreateSummonModel model);
        Task<bool> UpdateValidation(UpdateSummonModel model);
        Task<bool> DeleteValidation(int summonId);
        Task<bool> EnableValidation(int summonId);
        Task<bool> DisableValidation(int summonId);
    }
}
