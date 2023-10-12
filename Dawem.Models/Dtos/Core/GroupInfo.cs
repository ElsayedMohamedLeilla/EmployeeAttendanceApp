namespace Dawem.Models.Dtos.Core
{
    public class GroupInfo
    {
        public GroupInfo()
        {

        }
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public int MainBranchId { get; set; }
        public bool IsActive { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? GlobalName { get; set; }

    }


}
