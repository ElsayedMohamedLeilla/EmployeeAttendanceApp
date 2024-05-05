using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(FingerprintDevice) + LeillaKeys.S)]
    public class FingerprintDevice : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string PortNumber { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public DateTime LastSeenDateUTC { get; set; }
    }
}
