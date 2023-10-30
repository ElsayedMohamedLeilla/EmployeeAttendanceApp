using Dawem.Models.Dtos.Employees.HolidayType;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IHolidayTypeBLValidation
    {
        Task<bool> CreateValidation(CreateHolidayTypeModel model);
        Task<bool> UpdateValidation(UpdateHolidayTypeModel model);
    }
}
