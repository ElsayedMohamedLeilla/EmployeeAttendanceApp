namespace Dawem.Models.Dtos.Dawem.Shared
{
    public class VerifyEmailModel
    {
        public List<string> Emails { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string UserName { get; set; }

    }
}
