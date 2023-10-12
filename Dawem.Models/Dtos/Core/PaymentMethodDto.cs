using Dawem.Models.Dtos.Provider;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Models.Dtos.Core
{
    public class PaymentMethodDTO
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


        public string? Notes { get; set; }
        public PaymentMethodType? PaymentMethodType { get; set; }
        public List<PaymentMethodBranchDTO>? PaymentMethodBranches { get; set; }
        [NotMapped]
        public int[]? PaymentMethodBranchesIdList { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
