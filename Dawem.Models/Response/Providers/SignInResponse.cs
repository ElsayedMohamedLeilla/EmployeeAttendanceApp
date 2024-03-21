using Dawem.Models.Dtos.Identities;

namespace Dawem.Models.Response.Provider
{
    public class SignInResponse : BaseResponse
    {
        public TokenDto? TokeObject { get; set; }
    }
}
