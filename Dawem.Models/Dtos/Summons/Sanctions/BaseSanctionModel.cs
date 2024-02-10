using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Summons.Sanctions
{
    public class BaseSanctionModel
    {
        public string Name { get; set; }
        public SanctionType Type { get; set; }
        public bool IsActive { get; set; }
        public string WarningMessage { get; set; }
    }
}
