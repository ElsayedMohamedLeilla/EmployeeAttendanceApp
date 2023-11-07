using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IScheduleBLValidation
    {
        Task<bool> CreateValidation(CreateScheduleModel model);
        Task<bool> UpdateValidation(UpdateScheduleModel model);
    }
}
