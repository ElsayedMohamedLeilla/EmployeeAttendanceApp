namespace Dawem.Domain.Entities
{
    public interface IBaseEntity
    {
        DateTime AddedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        int? AddUserId { get; set; }
        int? ModifyUserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletionDate { get; set; }
        
    }
}