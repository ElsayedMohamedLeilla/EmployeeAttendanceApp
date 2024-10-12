using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes
{
    public class GetDefaultOfficialHolidaysInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<NameTranslationGetInfoModel> NameTranslations { get; set; }

    }
}
