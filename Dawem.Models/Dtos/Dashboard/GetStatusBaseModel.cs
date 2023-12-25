using Dawem.Enums.Generals;
using Dawem.Models.Criteria;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Dashboard
{
    public class GetStatusBaseModel : BaseCriteria
    {
        public GetRequestsStatusType? Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [JsonIgnore]
        public DateTime LocalDate { get; set; }

    }
}
