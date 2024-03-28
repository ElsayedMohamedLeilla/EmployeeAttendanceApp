using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;

namespace Dawem.Contract.BusinessValidation.Dawem.Employees
{
    public interface IHolidayTypeBLValidation
    {
        Task<bool> CreateValidation(CreateHolidayTypeModel model);
        Task<bool> UpdateValidation(UpdateHolidayTypeModel model);
    }
}
