using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestAttachment) + LeillaKeys.S)]
    public class RequestAttachment : BaseEntity
    {
        #region Foregn Keys

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public Request Request { get; set; }

        #endregion

        public string FileName { get; set; }
    }
}
