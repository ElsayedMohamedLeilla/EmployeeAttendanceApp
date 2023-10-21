
using Microsoft.AspNetCore.Http;

namespace Dawem.Models.Dtos.Provider
{
    public class UpdateDepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
