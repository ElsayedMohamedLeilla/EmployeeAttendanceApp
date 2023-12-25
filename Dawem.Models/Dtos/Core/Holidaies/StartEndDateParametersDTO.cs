using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Core.Holidaies
{
    public class StartEndDateParametersDTO
    {
        public int startYear { get; set; }
        public  int startMonth { get; set; }
        public int startDay { get; set; }
        public  int endYear { get; set; }
        public  int endMonth { get; set; }
        public  int endDay { get; set; }
        public DateType dateType { get; set; }


    }
}
