using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawem.Models.Dtos.Core.JustificationsTypes
{
    public class CreateJustificationsTypeDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
