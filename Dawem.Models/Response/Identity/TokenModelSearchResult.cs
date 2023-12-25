using Dawem.Models.Dtos.Identities;

namespace Dawem.Models.Response.Identity
{
    public class TokenModelSearchResult
    {
        public TokenDto? TokenData { get; set; }
        public string? AuthToken { get; set; }
    }
}
