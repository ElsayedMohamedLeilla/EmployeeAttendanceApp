using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Others.VacationBalances
{
    public class UpdateVacationBalanceModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public DefaultVacationType DefaultVacationType { get; set; }
        public float Balance { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
    }
}
