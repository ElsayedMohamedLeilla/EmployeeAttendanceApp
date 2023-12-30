using Dawem.Enums.Generals;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Dawem.Models.Dtos.Core.Holidays
{
    public class CreateHolidayDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateType DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public bool IsSpecifiedByYear { get; set; }

      


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
