using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Core.VacationsTypes
{
    public class GetVacationsTypeResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public VacationType Type { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
