namespace Dawem.Models.Dtos.Shared
{
    public class VerifyEmailModel : BaseResponse
    {
        //public string verificationCode { get; set; }

        public string Email { get; set; }
        public string Subject { get; set; }

        public string Body { get; set; }
        public string UserName { get; set; }

    }
}
