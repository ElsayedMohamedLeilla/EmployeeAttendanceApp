using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestVacation) + LeillaKeys.S)]
    public class RequestVacation : BaseEntity
    {
        #region Foregn Keys

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public Request Request { get; set; }

        public int VacationTypeId { get; set; }
        [ForeignKey(nameof(VacationTypeId))]
        public VacationType VacationType { get; set; }

        #endregion

        public int Code { get; set; }
        public int NumberOfDays { get; set; }
        public float BalanceBeforeRequest { get; set; }
        public float BalanceAfterRequest { get; set; }
        public DateTime DateTo { get; set; }        
    }
}
