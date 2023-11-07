using Dawem.Domain.Entities.Provider;
using Dawem.Enums.General;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendance
{
    [Table(nameof(WeekAttendanceShift) + LeillaKeys.S)]
    public class WeekAttendanceShift : BaseEntity
    {
        #region Foregn Keys

        public int WeekAttendanceId { get; set; }
        [ForeignKey(nameof(WeekAttendanceId))]
        public WeekAttendance WeekAttendance { get; set; }

        public int? ShiftId { get; set; }
        [ForeignKey(nameof(ShiftId))]
        public ShiftWorkingTime Shift { get; set; }

        #endregion
        public WeekDays WeekDay { get; set; }

    }
}
