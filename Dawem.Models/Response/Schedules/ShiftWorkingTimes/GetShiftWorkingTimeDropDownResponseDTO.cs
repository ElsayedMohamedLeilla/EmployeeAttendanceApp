namespace Dawem.Models.Response.Schedules.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeDropDownResponseDTO
    {
        public List<GetShiftWorkingTimeForDropDownResponseModelDTO> ShiftWorkingTimes { get; set; }
        public int TotalCount { get; set; }
    }
}
