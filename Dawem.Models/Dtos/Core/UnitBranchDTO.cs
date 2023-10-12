namespace Dawem.Models.Dtos.Core
{
    public class UnitBranchDTO
    {
        public int Id { get; set; }
        public int BranchId { get; set; }

        //public virtual BranchDTO? Branch { get; set; }
        public int UnitId { get; set; }

        //public UnitDTO? Unit { get; set; }

        public string? UnitGlobalName { get; set; }
        public string? BranchGlobalName { get; set; }
    }
}
