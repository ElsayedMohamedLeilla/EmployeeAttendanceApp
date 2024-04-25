using Dawem.Domain.Entities.Providers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.UserManagement
{
    public class EmployeeOTP : BaseEntity
    {
        public int OTPCount { get; set; }
        public int EmployeeId { get; set; }
        public int OTP { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsVerified { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}