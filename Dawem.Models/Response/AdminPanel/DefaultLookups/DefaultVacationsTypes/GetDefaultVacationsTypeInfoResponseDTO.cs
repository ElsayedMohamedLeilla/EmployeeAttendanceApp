using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultVacationsTypes
{
    public class GetDefaultVacationsTypeInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public DefaultVacationType DefaultType { get; set; }
        public string DefaultTypeName { get; set; }
        public bool IsActive { get; set; }
        public List<NameTranslationGetInfoModel> NameTranslations { get; set; }

    }
}
