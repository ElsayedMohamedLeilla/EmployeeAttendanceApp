namespace Dawem.Models.Dtos.Providers
{
    public class SignUpModel
    {
        #region Company Data
        public string CompanyName { get; set; }
        public int CompanyCountryId { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public int NumberOfEmployees { get; set; }
        public int SubscriptionDurationInMonths { get; set; }
        public int? PlanId { get; set; }

        #endregion

        #region UserData

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserEmail { get; set; }
        public string UserMobileNumber { get; set; }
        public string Name { get; set; }
        public bool Agreed { get; set; }

        #endregion
    }
}
