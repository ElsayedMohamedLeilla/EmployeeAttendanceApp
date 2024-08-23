using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes
{
    public class CreateDefaultVacationsTypeDTO : BaseCreateAndUpdateNameTranslation
    {
        public string Name { get; set; }
        public DefaultVacationType DefaultType { get; set; }
        public bool IsActive { get; set; }
    }
}
