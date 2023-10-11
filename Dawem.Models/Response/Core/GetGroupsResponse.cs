using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class GetGroupsResponse : BaseResponse
    {
        public List<GroupDTO?>? Groups { get; set; }
    }
}
