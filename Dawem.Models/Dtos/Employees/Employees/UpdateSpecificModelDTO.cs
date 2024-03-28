using Dawem.Enums.Generals;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateSpecificModelDTO
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public IFormFile ProfileImageFile { get; set; }
        public string ProfileImageName { get; set; }


    }
}