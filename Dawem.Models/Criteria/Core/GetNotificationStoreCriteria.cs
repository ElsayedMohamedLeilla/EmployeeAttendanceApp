using Newtonsoft.Json;

namespace Dawem.Models.Criteria.Core
{
    public class GetNotificationStoreCriteria : BaseCriteria
    {
        public bool? IsRead { get; set; }
        [JsonIgnore]
        public int EmployeeId { get; set; }
    }
}
