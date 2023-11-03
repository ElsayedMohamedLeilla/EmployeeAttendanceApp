using Dawem.Domain.Entities.Provider;
using Dawem.Enums.General;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendance
{
    [Table(nameof(WeekAttendanceShift) + DawemKeys.S)]
    public class WeekAttendanceShift : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int WeekAttendanceId { get; set; }
        [ForeignKey(nameof(WeekAttendanceId))]
        public WeekAttendance WeekAttendance { get; set; }
        public int? ShiftId { get; set; }

        #endregion
        public WeekDays WeekDay { get; set; }

    }
}
