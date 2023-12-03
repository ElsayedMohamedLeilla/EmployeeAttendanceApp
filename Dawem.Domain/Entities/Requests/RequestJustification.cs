using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestJustification) + LeillaKeys.S)]
    public class RequestJustification : BaseEntity
    {
        #region Foregn Keys

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public Request Request { get; set; }

        public int JustificationTypeId { get; set; }
        [ForeignKey(nameof(JustificationTypeId))]
        public JustificationType JustificatioType { get; set; }

        #endregion

        public int Code { get; set; }
    }
}
