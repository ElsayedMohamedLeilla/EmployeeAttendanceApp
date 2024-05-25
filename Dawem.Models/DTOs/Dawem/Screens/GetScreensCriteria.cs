using Dawem.Models.Criteria;

namespace Dawem.Models.DTOs.Dawem.Screens
{
    public class GetScreensCriteria : BaseCriteria
    {
        public int? ScreenCode { get; set; }
        public int? ActionCode { get; set; }
    }
}
