using Dawem.Models.Dtos.Dawem.Summons.Summons;

namespace Dawem.Contract.BusinessValidation.Dawem.Summons
{
    public interface ISummonBLValidation
    {
        Task<bool> CreateValidation(CreateSummonModel model);
        Task<bool> UpdateValidation(UpdateSummonModel model);
    }
}
