namespace Dawem.Models.Dtos.Core.JustificationsTypes
{
    public class UpdateJustificationsTypeDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
