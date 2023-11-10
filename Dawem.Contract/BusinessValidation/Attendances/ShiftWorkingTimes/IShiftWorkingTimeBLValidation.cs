using Dawem.Models.Dtos.Employees.Attendance.ShiftWorkingTimes;

namespace Dawem.Contract.BusinessValidation.Attendances.ShiftWorkingTimes
{
    public interface IShiftWorkingTimeBLValidation
    {
        Task<bool> CreateValidation(CreateShiftWorkingTimeModelDTO model);
        Task<bool> UpdateValidation(UpdateShiftWorkingTimeModelDTO model);
    }
}
