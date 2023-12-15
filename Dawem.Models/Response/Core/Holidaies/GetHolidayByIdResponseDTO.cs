using Dawem.Enums.Generals;
using System.Globalization;

namespace Dawem.Models.Response.Core.Holidaies
{
    public class GetHolidayByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateType DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }

        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }

    }
}
