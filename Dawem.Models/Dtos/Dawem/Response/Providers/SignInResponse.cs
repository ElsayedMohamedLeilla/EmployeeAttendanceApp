using Dawem.Models.Dtos.Dawem.Identities;

namespace Dawem.Models.Response.Providers
{
    public class SignInResponse : BaseResponse
    {
        public TokenDto TokeObject { get; set; }
    }
}
