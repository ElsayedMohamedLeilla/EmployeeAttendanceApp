namespace Dawem.Models.Dtos.Providers
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string UserEmail { get; set; }

    }
}
