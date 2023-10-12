namespace Dawem.Models.Dtos.Core
{
    public class UnitDTO
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public int MainBranchId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? GlobalName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public IList<ItemUnit>? ProductUnits { get; set; }

        public List<UnitBranchDTO>? UnitBranches { get; set; }


    }
}
