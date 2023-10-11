using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.General;

namespace SmartBusinessERP.Models.Context
{
    public class RequestHeaderContext
    {
        public User? User { get; set; }

        public string? Lang { get; set; }
        public int? UserId { get; set; }
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public AddedByDevice AddedByDevice { get; set; }
        public string? RequestProtocol { get; set; }
        public string? RequestHost { get; set; }
        public string? RequestPath { get; set; }
        public int? RequestPort { get; set; }
        public string? BaseUrl { get; set; }
        public bool IsMainBranch { get; set; }
    }
}
