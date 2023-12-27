using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Core.Holidays
{
    public class StartEndDateParametersDTO
    {
        //public int startYear { get; set; }
        //public int startMonth { get; set; }
        //public int startDay { get; set; }
        //public int endYear { get; set; }
        //public int endMonth { get; set; }
        //public int endDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateType dateType { get; set; }

        public bool IsSpecificByYear { get; set; }


    }
}
