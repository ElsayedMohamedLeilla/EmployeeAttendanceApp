using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Core.VacationsTypes
{
    public class CreateVacationsTypeDTO
    {
        public string Name { get; set; }
        public VacationType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
