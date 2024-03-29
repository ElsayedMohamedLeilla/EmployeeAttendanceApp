using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Core.VacationsTypes
{
    public class CreateVacationsTypeDTO
    {
        public string Name { get; set; }
        public DefaultVacationType DefaultType { get; set; }
        public bool IsActive { get; set; }
    }
}
