using Dawem.Domain.Entities.Provider;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(SchedulePlanBackgroundJobLog) + LeillaKeys.S)]
    public class SchedulePlanBackgroundJobLog : BaseEntity
    {
        public SchedulePlanBackgroundJobLog()
        {
            SchedulePlanBackgroundJobLogEmployees = new List<SchedulePlanBackgroundJobLogEmployee>();
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
        public List<SchedulePlanBackgroundJobLogEmployee> SchedulePlanBackgroundJobLogEmployees { get; set; }

    }
}
