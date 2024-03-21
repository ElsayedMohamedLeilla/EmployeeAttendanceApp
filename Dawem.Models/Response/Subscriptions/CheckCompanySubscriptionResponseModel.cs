using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Employees.Departments
{
    public class CheckCompanySubscriptionResponseModel
    {
        public CheckCompanySubscriptionResponseModel()
        {
            Result = true;
        }
        public bool Result { get; set; }
        public CheckCompanySubscriptionErrorType ErrorType { get; set; }
    }
}
