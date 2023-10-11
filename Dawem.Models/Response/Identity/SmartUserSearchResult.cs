using SmartBusinessERP.Models.Dtos.Identity;

namespace SmartBusinessERP.Models.Response.Identity
{
    public class SmartUserSearchResult : BaseResponse
    {
        public List<SmartUserDTO>? SmartUsers { get; set; }
    }
}
