using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(ScheduleDay) + LeillaKeys.S)]
    public class ScheduleDay : BaseEntity
    {
        #region Foregn Keys

        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Schedule Schedule { get; set; }

        public int? ShiftId { get; set; }
        [ForeignKey(nameof(ShiftId))]
        public ShiftWorkingTime Shift { get; set; }

        #endregion
        public WeekDay WeekDay { get; set; }

    }
}
