using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Schedules;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(Group) + LeillaKeys.S)]
    public class Group : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int? ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public Employee GroupManager { get; set; }
        #endregion

        public int Code { get; set; }
        public string Name { get; set; }

        public List<GroupEmployee> GroupEmployees { get; set; }
       
        public List<GroupManagerDelegator> GroupManagerDelegators { get; set; }
        public List<ZoneGroup> Zones { get; set; }
        public List<SchedulePlanGroup> SchedulePlanGroups { get; set; }




    }
}
