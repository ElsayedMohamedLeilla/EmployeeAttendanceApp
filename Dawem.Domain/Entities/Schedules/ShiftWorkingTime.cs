using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(ShiftWorkingTime) + LeillaKeys.S)]
    public class ShiftWorkingTime : BaseEntity
    {
        #region Foregn Keys
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public AmPm TimePeriod { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public int AllowedMinutes { get; set; }
        public bool IsTwoDaysShift { get; set; }
        public List<ScheduleDay> ScheduleDays { get; set; }
    }
}
