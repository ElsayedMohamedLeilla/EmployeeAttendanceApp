namespace Dawem.Models.Dtos.Shared
{
    public class VerifyEmailModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }

    }
}
