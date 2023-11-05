using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Models.Response.Core.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public double AllowedMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}
