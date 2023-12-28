using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;

namespace Dawem.Models.Context
{
    public class RequestInfo
    {
        public MyUser User { get; set; }
        public string Lang { get; set; }
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public ApplicationType AddedByDevice { get; set; }
        public string RequestProtocol { get; set; }
        public string RemoteIpAddress { get; set; }
        public string RequestHost { get; set; }
        public string RequestPath { get; set; }
        public int? RequestPort { get; set; }
        public string BaseUrl { get; set; }
        public bool IsMainBranch { get; set; }
        public DateTime LocalDateTime { get; set; }

    }
}
