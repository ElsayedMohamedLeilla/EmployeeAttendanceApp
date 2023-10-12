using Dawem.Models.Dtos.Identity;

namespace Dawem.Models.Response.Identity
{
    public class SmartUserSearchResult : BaseResponse
    {
        public List<SmartUserDTO>? SmartUsers { get; set; }
    }
}
