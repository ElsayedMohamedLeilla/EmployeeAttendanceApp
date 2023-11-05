using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IShiftWorkingTimeBLValidation
    {
        Task<bool> CreateValidation(CreateShiftWorkingTimeModelDTO model);
        Task<bool> UpdateValidation(UpdateShiftWorkingTimeModelDTO model);
    }
}
