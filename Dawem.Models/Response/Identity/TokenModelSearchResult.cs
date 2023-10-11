using SmartBusinessERP.Models.Dtos.Identity;

namespace SmartBusinessERP.Models.Response.Identity
{
    public class TokenModelSearchResult : BaseResponse
    {
        public TokenDto? TokenData { get; set; }

        public string? AuthToken { get; set; }
    }
}
