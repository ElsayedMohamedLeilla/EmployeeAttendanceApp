using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class GetGroupInfoResponse : BaseResponse
    {
        public GroupInfo? GroupInfo { get; set; }
    }
}
