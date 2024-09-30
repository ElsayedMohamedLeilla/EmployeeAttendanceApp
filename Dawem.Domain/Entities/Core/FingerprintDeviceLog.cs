using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Schedules;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(FingerprintDeviceLog) + LeillaKeys.S)]
    public class FingerprintDeviceLog : BaseEntity
    {
        #region Forign Key
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public string DeviceSerialNumber { get; set; }
        public string LogType { get; set; }
        public DateTime DateUTC { get; set; }
        public string RequestBody { get; set; }
        public string DeviceLogType { get; set; }
    }
}
