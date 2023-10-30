namespace Dawem.Models.Dtos.Core.VacationsTypes
{
    public class UpdateVacationsTypeDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
