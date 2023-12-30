using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Core.Holidays
{
    public class StartEndDateParametersDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateType dateType { get; set; }

        public bool IsSpecificByYear { get; set; }


    }
}
