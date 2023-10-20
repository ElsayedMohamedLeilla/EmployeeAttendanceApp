namespace Dawem.Models.Dtos.Provider
{
    public class SignUpModel
    {
        #region Company Data
        public string CompanyName { get; set; }
        public int CompanyCountryId { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }

        #endregion

        #region UserData

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserEmail { get; set; }
        public string UserMobileNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Agreed { get; set; }

        #endregion
    }
}
