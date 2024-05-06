using Newtonsoft.Json;

namespace Dawem.Models.Criteria.Core
{
    public class GetNotificationCriteria : BaseCriteria
    {
        public bool? IsRead { get; set; }
        [JsonIgnore]
        public int EmployeeId { get; set; }
    }
}
