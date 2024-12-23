using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Core.VacationsTypes
{
    public class GetOvertimesTypeByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
