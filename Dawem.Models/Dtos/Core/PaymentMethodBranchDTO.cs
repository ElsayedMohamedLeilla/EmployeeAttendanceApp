namespace Dawem.Models.Dtos.Core
{
    public class PaymentMethodBranchDTO
    {
        public int Id { get; set; }
        public int BranchId { get; set; }

        //public virtual BranchDTO? Branch { get; set; }
        public int PaymentMethodId { get; set; }

        //public PaymentMethodDTO? PaymentMethod { get; set; }

        public string? PaymentMethodGlobalName { get; set; }
        public string? BranchGlobalName { get; set; }
    }
}
