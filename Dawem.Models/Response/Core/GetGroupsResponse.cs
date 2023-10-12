using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class GetGroupsResponse : BaseResponse
    {
        public List<GroupDTO?>? Groups { get; set; }
    }
}
