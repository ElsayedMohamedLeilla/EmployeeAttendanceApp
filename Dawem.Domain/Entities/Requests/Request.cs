using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Provider;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(Request) + LeillaKeys.S)]
    public class Request : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
        public int? DecisionUserId { get; set; }
        [ForeignKey(nameof(DecisionUserId))]
        public MyUser DecisionUser { get; set; }
        public RequestVacation RequestVacation { get; set; }
        public RequestAssignment RequestAssignment { get; set; }
        public RequestTask RequestTask { get; set; }
        public RequestJustification RequestJustification { get; set; }
        public RequestPermission RequestPermission { get; set; }

        #endregion
        public int Code { get; set; }
        public RequestType Type { get; set; }
        public bool IsNecessary { get; set; }
        public bool ForEmployee { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime? DecisionDate { get; set; }
        public DateTime Date { get; set; }
        public string RefuseReason { get; set; }
        public List<RequestAttachment> RequestAttachments { get; set; }

       
    }
}
