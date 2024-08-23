using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Dawem.Shared;

namespace Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes
{
    public class UpdateDefaultVacationsTypeDTO : BaseCreateAndUpdateNameTranslation
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public DefaultVacationType DefaultType { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
