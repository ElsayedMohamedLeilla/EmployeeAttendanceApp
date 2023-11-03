using Dawem.Domain.Entities.Provider;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendance
{
    [Table(nameof(WeekAttendance) + DawemKeys.S)]
    public class WeekAttendance : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public List<WeekAttendanceShift> WeekAttendanceShifts { get; set; }
    }
}
