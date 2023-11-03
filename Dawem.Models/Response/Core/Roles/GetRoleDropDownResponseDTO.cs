using Dawem.Models.Response.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Models.Response.Core.Roles
{
    public class GetRoleDropDownResponseDTO
    {
        public List<GetRoleForDropDownResponseModelDTO> Roles { get; set; }
        public int TotalCount { get; set; }
    }
}
