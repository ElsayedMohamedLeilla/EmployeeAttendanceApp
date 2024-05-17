using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Schedules.Schedules;

namespace Dawem.Models.Response.Dawem.Attendances
{
    public class CheckIfShiftIsTwoDaysShiftResponseModel
    {
        public DateTime ClientLocalDateTime { get; set; }
        public ShiftInfoModel ShiftInfo { get; set; }
        public GetScheduleModel Schedule { get; set; }
    }
}
