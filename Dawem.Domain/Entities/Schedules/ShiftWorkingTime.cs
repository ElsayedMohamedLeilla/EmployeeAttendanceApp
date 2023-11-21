using Dawem.Domain.Entities.Provider;
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
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public double AllowedMinutes { get; set; }



    }
}
