using Dawem.Domain.Entities.Provider;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(Group) + "s")]
    public class Group : BaseEntity
    {
        public int MainBranchId { get; set; }

        [ForeignKey(nameof(MainBranchId))]
        public virtual Branch? MainBranch { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
    }
}
