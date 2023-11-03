using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.BusinessValidation.Employees
{
    public interface IWeekAttendanceBLValidation
    {
        Task<bool> CreateValidation(CreateWeekAttendanceModel model);
        Task<bool> UpdateValidation(UpdateWeekAttendanceModel model);
    }
}
