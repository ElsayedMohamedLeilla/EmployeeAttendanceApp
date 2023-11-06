using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Models.Response.Core.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public double AllowedMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}
