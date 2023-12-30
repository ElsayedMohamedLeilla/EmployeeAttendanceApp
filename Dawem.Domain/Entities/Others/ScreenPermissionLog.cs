using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Configration;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(ScreenPermissionLog) + LeillaKeys.S)]
    public class ScreenPermissionLog : BaseEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Company User { get; set; }
        public ApplicationScreenCode ScreenCode { get; set; }
        public Enums.Configration.ApplicationAction ActionType { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
