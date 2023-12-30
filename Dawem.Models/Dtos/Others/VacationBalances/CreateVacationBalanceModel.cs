using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Others.VacationBalances
{
    public class CreateVacationBalanceModel
    {
        public ForType ForType { get; set; }
        public int? EmployeeId { get; set; }
        public int? GroupId { get; set; }
        public int? DepartmentId { get; set; }
        public int Year { get; set; }
        public DefaultVacationType DefaultVacationType { get; set; }
        public float Balance { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
    }
}
