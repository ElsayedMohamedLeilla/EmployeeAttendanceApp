using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultPenalties
{
    public class GetDefaultPenaltiesInfoResponseDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<NameTranslationGetInfoModel> NameTranslations { get; set; }

    }
}
