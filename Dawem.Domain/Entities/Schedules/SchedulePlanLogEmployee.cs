using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(SchedulePlanLogEmployee) + LeillaKeys.S)]
    public class SchedulePlanLogEmployee : BaseEntity
    {
        #region Foregn Keys

        public int SchedulePlanLogId { get; set; }
        [ForeignKey(nameof(SchedulePlanLogId))]
        public SchedulePlanLog SchedulePlanLog { get; set; }
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
