using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestTaskEmployee) + LeillaKeys.S)]
    public class RequestTaskEmployee : BaseEntity
    {
        #region Foregn Keys

        public int RequestTaskId { get; set; }
        [ForeignKey(nameof(RequestTaskId))]
        public RequestTask RequestTask { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        #endregion
    }
}
