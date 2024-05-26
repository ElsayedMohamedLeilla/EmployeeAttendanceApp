using Dawem.Domain.Entities.Others;
using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(ScreenAction) + LeillaKeys.S)]
    public class ScreenAction : BaseEntity
    {
        public int ScreenId { get; set; }
        [ForeignKey(nameof(ScreenId))]
        public Screen Screen { get; set; }
        public ApplicationActionCode ActionCode { get; set; }
        public string ActionCodeName { get; set; }
    }

}
