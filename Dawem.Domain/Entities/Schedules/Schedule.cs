using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Schedules
{
    [Table(nameof(Schedule) + LeillaKeys.S)]
    public class Schedule : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public List<ScheduleDay> ScheduleDays { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
