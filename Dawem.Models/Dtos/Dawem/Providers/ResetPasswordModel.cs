﻿namespace Dawem.Models.Dtos.Dawem.Providers
{
    public class ResetPasswordModel
    {
        public string ResetToken { get; set; }
        public string UserEmail { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
