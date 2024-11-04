using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Schedules;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(Department) + LeillaKeys.S)]
    public class Department : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Department Parent { get; set; }

        public int? ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public Employee Manager { get; set; }

        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public List<Department> Children { get; set; }
        public List<Employee> Employees { get; set; }
        public List<ZoneDepartment> Zones { get; set; }
        public List<DepartmentManagerDelegator> ManagerDelegators { get; set; }
        public List<SchedulePlanDepartment> SchedulePlanDepartments { get; set; }
        public bool AllowFingerprintOutsideAllowedZones { get; set; }
        public bool InsertedFromExcel { get; set; }
    }
}