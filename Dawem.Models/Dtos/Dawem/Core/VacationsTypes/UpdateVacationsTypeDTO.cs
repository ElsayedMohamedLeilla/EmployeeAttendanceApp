using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Core.VacationsTypes
{
    public class UpdateVacationsTypeDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public DefaultVacationType DefaultType { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
