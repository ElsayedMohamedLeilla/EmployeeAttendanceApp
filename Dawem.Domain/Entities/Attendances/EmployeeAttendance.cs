using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Schedules;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendances
{
    [Table(nameof(EmployeeAttendance) + LeillaKeys.S)]
    public class EmployeeAttendance : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Schedule Schedule { get; set; }
        public int ShiftId { get; set; }
        [ForeignKey(nameof(ShiftId))]
        public ShiftWorkingTime Shift { get; set; }

        #endregion
        public int Code { get; set; }
        public DateTime LocalDate { get; set; }
        public TimeOnly ShiftCheckInTime { get; set; }
        public TimeOnly ShiftCheckOutTime { get; set; }
        public int AllowedMinutes { get; set; }
        public List<EmployeeAttendanceCheck> EmployeeAttendanceChecks { get; set; }

        public bool InsertedFromExcel { get; set; }
    }
}
