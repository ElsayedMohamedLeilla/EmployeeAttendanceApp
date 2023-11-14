using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Models.Response.Attendances.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeResponseDTO
    {
        public List<GetShiftWorkingTimeResponseModelDTO> ShiftWorkingTimes { get; set; }
        public int TotalCount { get; set; }
    }
}
