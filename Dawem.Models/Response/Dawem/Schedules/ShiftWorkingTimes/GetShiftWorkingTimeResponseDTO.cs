namespace Dawem.Models.Response.Dawem.Schedules.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeResponseDTO
    {
        public List<GetShiftWorkingTimeResponseModelDTO> ShiftWorkingTimes { get; set; }
        public int TotalCount { get; set; }
    }
}
