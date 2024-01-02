using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(SchedulePlanLog) + LeillaKeys.S)]
    public class SchedulePlanLog : BaseEntity
    {
        public SchedulePlanLog()
        {
            SchedulePlanLogEmployees = new List<SchedulePlanLogEmployee>();
        }
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int SchedulePlanId { get; set; }
        [ForeignKey(nameof(SchedulePlanId))]
        public SchedulePlan SchedulePlan { get; set; }

        #endregion
        public int Code { get; set; }
        public ForType SchedulePlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public List<SchedulePlanLogEmployee> SchedulePlanLogEmployees { get; set; }

    }
}
