namespace Dawem.Models.Dtos.Dawem.Core.Responsibilities
{
    public class UpdateResponsibilityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ForEmployeesApplication { get; set; }
        public bool IsActive { get; set; }
    }
}
