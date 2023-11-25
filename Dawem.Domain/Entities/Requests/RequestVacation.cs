using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
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

        public DateTime DateTo { get; set; }
    }
}
