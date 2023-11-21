using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(SchedulePlanGroup) + LeillaKeys.S)]
    public class SchedulePlanGroup : BaseEntity
    {
        #region Foregn Keys

        public int SchedulePlanId { get; set; }
        [ForeignKey(nameof(SchedulePlanId))]
        public SchedulePlan SchedulePlan { get; set; }

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        #endregion
    }
}
