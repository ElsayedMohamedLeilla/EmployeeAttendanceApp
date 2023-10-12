using Dawem.Models.Dtos.Identity;

namespace Dawem.Models.Response.Provider
{
    public class SignInResponse : BaseResponse
    {
        public TokenDto? TokeObject { get; set; }
    }
}
