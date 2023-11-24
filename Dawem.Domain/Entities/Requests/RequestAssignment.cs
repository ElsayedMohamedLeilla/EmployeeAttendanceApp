using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestAssignment) + LeillaKeys.S)]
    public class RequestAssignment : BaseEntity
    {
        #region Foregn Keys

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public Request Request { get; set; }

        public int AssignmentTypeId { get; set; }
        [ForeignKey(nameof(AssignmentTypeId))]
        public AssignmentType AssignmentType { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        #endregion

        public DateTime DateTo { get; set; }
    }
}
