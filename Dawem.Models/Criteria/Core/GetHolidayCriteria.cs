using Dawem.Enums.Generals;

namespace Dawem.Models.Criteria.Core
{
    public class GetHolidayCriteria : BaseCriteria
    {
        public int? Year { get; set; }
        public DateType? DateType { get; set; }


    }
}
