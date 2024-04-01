using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;

namespace Dawem.Validation.BusinessValidation.Dawem.Requests
{

    public class RequestBLValidation : IRequestBLValidation
    {
        private readonly RequestInfo requestInfo;
        public RequestBLValidation(RequestInfo _requestInfo)
        {
            requestInfo = _requestInfo;
        }
        public async Task<bool> IsEmployeeValidation()
        {
            if (requestInfo.EmployeeId == 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);
            }

            return true;
        }
    }
}
