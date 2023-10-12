using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Dtos.Core
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public int MainBranchId { get; set; }
        public virtual BranchDTO? MainBranch { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? GlobalName { get; set; }
        public bool IsActive { get; set; }
    }
}
