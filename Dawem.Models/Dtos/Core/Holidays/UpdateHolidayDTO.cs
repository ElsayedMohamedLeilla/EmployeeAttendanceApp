using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Core.Holidays
{
    public class UpdateHolidayDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateType DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public bool IsSpecifiedByYear { get; set; }


        //[JsonIgnore]
        //public int StartDay { get; set; }
        //[JsonIgnore]
        //public int EndDay { get; set; }
        //[JsonIgnore]
        //public int StartMonth { get; set; }
        //[JsonIgnore]
        //public int EndMonth { get; set; }
        //[JsonIgnore]
        //public int? StartYear { get; set; }
        //[JsonIgnore]
        //public int? EndYear { get; set; }


        public void JustifyStartEndDate()
        {
            if (!IsSpecifiedByYear)
            {
                DateTime tempStartDate = StartDate;
                DateTime tempEndDate = EndDate;
                StartDate = new DateTime(0001, tempStartDate.Month, tempStartDate.Day);
                EndDate = new DateTime(0001, tempEndDate.Month, tempEndDate.Day);
            }
        }
    }
}
