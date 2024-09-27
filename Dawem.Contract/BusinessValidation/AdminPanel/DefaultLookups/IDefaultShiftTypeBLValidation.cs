using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes;

namespace Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups
{
    public interface IDefaultShiftTypeBLValidation
    {
        Task<bool> CreateValidation(CreateDefaultShiftsTypeDTO model);
        Task<bool> UpdateValidation(UpdateDefaultShiftsTypeDTO model);
    }
}
