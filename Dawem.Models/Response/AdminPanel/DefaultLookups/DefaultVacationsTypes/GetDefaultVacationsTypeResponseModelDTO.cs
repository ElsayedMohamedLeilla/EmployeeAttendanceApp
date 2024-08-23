using Dawem.Enums.Generals;

namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultVacationsTypes
{
    public class GetDefaultVacationsTypeResponseModelDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public DefaultVacationType DefaultType { get; set; }
        public string DefaultTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
