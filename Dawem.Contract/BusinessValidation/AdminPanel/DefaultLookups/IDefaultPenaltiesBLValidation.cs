using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultPenaltiesBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultPenaltiesDTO model);
        Task<bool> UpdateValidation(UpdateDefaultPenaltiesDTO model);
    }
}
