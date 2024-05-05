using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(FingerprintTransaction) + LeillaKeys.S)]
    public class FingerprintTransaction : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
        public int? ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Schedule Schedule { get; set; }
        public int FingerprintDeviceId { get; set; }
        [ForeignKey(nameof(FingerprintDeviceId))]
        public FingerprintDevice FingerprintDevice { get; set; }
        #endregion
        public DateTime FingerprintDate { get; set; }
        public int FingerprintUserId { get; set; }
        public string SerialNumber { get; set; }
        public FingerPrintType FingerPrintType { get; set; }
    }
}
