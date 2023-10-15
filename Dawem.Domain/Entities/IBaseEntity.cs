namespace Dawem.Domain.Entities
{
    public interface IBaseEntity
    {
        int CompanyId { get; set; }
        int BranchId { get; set; }
        bool Status { get; set; }

        DateTime AddedDate { get; set; }
        DateTime? ModifiedDate { get; set; }

        int? AddUserId { get; set; }
        int? ModifyUserId { get; set; }




    }
}