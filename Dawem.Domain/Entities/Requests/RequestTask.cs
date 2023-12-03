using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Requests
{
    [Table(nameof(RequestTask) + LeillaKeys.S)]
    public class RequestTask : BaseEntity
    {
        #region Foregn Keys

        public int RequestId { get; set; }
        [ForeignKey(nameof(RequestId))]
        public Request Request { get; set; }

        public int TaskTypeId { get; set; }
        [ForeignKey(nameof(TaskTypeId))]
        public TaskType TaskType { get; set; }

        #endregion

        public int Code { get; set; }
        public DateTime DateTo { get; set; }
        public List<RequestTaskEmployee> TaskEmployees { get; set; }
    }
}
