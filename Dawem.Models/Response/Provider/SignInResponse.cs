using SmartBusinessERP.Models.Dtos.Identity;

namespace SmartBusinessERP.Models.Response.Provider
{
    public class SignInResponse : BaseResponse
    {
        public TokenDto? TokeObject { get; set; }
    }
}
