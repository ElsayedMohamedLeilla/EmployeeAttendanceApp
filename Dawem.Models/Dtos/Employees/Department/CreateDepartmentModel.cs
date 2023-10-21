
using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Provider
{
    public class CreateDepartmentModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
