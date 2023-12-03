using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestPermission) + LeillaKeys.S)]
    public class RequestPermission : BaseEntity
    {
        #region Foregn Keys

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public Request Request { get; set; }

        public int PermissionTypeId { get; set; }
        [ForeignKey(nameof(PermissionTypeId))]
        public PermissionType PermissionType { get; set; }

        #endregion

        public int Code { get; set; }
    }
}
