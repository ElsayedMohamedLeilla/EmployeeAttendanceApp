using Dawem.Models.Dtos.Summons.Summons;

namespace Dawem.Contract.BusinessValidation.Summons
{
    public interface ISummonBLValidation
    {
        Task<bool> CreateValidation(CreateSummonModel model);
        Task<bool> UpdateValidation(UpdateSummonModel model);
    }
}
