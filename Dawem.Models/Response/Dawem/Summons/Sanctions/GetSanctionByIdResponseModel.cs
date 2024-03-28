using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Summons.Sanctions
{
    public class GetSanctionByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public SanctionType Type { get; set; }
        public string WarningMessage { get; set; }
        public bool IsActive { get; set; }
    }
}
