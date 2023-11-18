using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendance
{
    [Table(nameof(SchedulePlanBackgroundJobLogEmployee) + LeillaKeys.S)]
    public class SchedulePlanBackgroundJobLogEmployee : BaseEntity
    {
        #region Foregn Keys

        public int SchedulePlanBackgroundJobLogId { get; set; }
        [ForeignKey(nameof(SchedulePlanBackgroundJobLogId))]
        public SchedulePlanBackgroundJobLog SchedulePlanBackgroundJobLog { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
        public int? OldScheduleId { get; set; }
        [ForeignKey(nameof(OldScheduleId))]
        public Schedule OldSchedule { get; set; }
        public int NewScheduleId { get; set; }
        [ForeignKey(nameof(NewScheduleId))]
        public Schedule NewSchedule { get; set; }

        #endregion

    }
}
