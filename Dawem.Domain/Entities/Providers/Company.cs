using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.Entities.Summons;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Providers
{
    [Table("Companies")]
    public class Company : BaseEntity
    {
        public string IdentityCode { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public virtual List<Branch> Branches { get; set; }
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }
        public string Email { get; set; }
        public List<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public List<Summon> Summons { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<SchedulePlan> SchedulePlans { get; set; }
        public int NumberOfEmployees { get; set; }
    }

}
