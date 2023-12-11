using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Provider;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(VacationsBalance) + LeillaKeys.S)]
    public class VacationsBalance : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        #endregion

        public int Year { get; set; }
        public DateTime ExpirationDate { get; set; }
        public VacationType VacationType { get; set; }
        public float Balance { get; set; }
        public float RemainingBalance { get; set; }
    }
}
