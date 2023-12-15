using Dawem.Domain.Entities.Provider;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(SchedulePlan) + LeillaKeys.S)]
    public class SchedulePlan : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Schedule Schedule { get; set; }

        public SchedulePlanEmployee SchedulePlanEmployee { get; set; }
        public SchedulePlanGroup SchedulePlanGroup { get; set; }
        public SchedulePlanDepartment SchedulePlanDepartment { get; set; }

        #endregion
        public int Code { get; set; }
        public ForType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }

    }
}
