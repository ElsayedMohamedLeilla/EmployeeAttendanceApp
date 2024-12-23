using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestOvertime) + LeillaKeys.S)]
    public class RequestOvertime : BaseEntity
    {
        #region Foregn Keys

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public Request Request { get; set; }
        public int OvertimeTypeId { get; set; }
        [ForeignKey(nameof(OvertimeTypeId))]
        public OvertimeType OvertimeType { get; set; }

        #endregion

        public int Code { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int OvertimeHoursCount { get; set; }
    }
}
