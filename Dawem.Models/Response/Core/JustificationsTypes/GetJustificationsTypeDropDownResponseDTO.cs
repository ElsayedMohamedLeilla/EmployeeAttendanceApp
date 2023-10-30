using Dawem.Models.Response.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Models.Response.Core.JustificationsTypes
{
    public class GetJustificationsTypeDropDownResponseDTO
    {
        public List<GetJustificationsTypeForDropDownResponseModelDTO> JustificationsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
