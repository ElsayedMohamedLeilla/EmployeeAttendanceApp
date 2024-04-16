using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Domain.Entities.Summons;
using Dawem.Domain.Entities.UserManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Providers
{
    [Table("Companies")]
    public class Company : BaseEntity
    {
        #region Relations
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }
        public int? PreferredLanguageId { get; set; }
        [ForeignKey(nameof(PreferredLanguageId))]
        public Language PreferredLanguage { get; set; }
        public List<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public List<Summon> Summons { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<SchedulePlan> SchedulePlans { get; set; }
        public List<CompanyAttachment> CompanyAttachments { get; set; }
        public virtual List<CompanyBranch> CompanyBranches { get; set; }
        public virtual List<CompanyIndustry> CompanyIndustries { get; set; }
        public Subscription Subscription { get; set; }
        public List<FingerprintDevice> FingerprintDevices { get; set; }
        public List<Zone> Zones { get; set; }
        public List<MyUser> Users { get; set; }
        #endregion
        public string IdentityCode { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string LogoImageName { get; set; }
        public string WebSite { get; set; }
        public string HeadquarterAddress { get; set; }
        public double? HeadquarterLocationLatitude { get; set; }
        public double? HeadquarterLocationLongtude { get; set; }
        public string HeadquarterPostalCode { get; set; }
        public string Email { get; set; }
        public int NumberOfEmployees { get; set; }
        public int TotalNumberOfEmployees { get; set; }
        public bool ImportDefaultData { get; set; }
    }
}
