using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(Plan) + LeillaKeys.S)]
    public class Plan : BaseEntity
    {
        public int Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int MinNumberOfEmployees { get; set; }
        public int MaxNumberOfEmployees { get; set; }
        public decimal EmployeeCost { get; set; }
        public int GracePeriodPercentage { get; set; } // will removed
        public bool IsTrial { get; set; }
    }

}
