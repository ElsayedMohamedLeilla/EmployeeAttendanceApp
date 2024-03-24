using Dawem.Models.Dtos.Identities;

namespace Dawem.Models.Response.Providers
{
    public class SignInResponse : BaseResponse
    {
        public TokenDto TokeObject { get; set; }
    }
}
