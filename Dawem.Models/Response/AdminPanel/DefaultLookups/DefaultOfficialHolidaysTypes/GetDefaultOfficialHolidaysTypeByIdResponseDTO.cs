using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes
{
    public class GetDefaultOfficialHolidaysTypeByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<NameTranslationModel> NameTranslations { get; set; }

    }
}
