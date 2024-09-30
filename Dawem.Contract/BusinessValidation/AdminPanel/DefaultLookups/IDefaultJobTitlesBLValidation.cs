using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultJobTitlesBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultJobTitlesDTO model);
        Task<bool> UpdateValidation(UpdateDefaultJobTitlesDTO model);
    }
}
