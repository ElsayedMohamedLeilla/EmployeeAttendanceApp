using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.UserManagement
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]


    public class EmployeeOTPController : DawemControllerBase
    {
        private readonly IEmployeeOTPBL employeeOTPBL;

        public EmployeeOTPController(IEmployeeOTPBL _employeeOTPBL)
        {
            employeeOTPBL = _employeeOTPBL;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> PreSignUp(PreSignUpDTO model)
        {
            string output = await employeeOTPBL.PreSignUp(model);
            return Success(output, messageCode: AmgadKeys.DoneOTPGeneratedSuccessfullyPleaseCheckYourEmailToGetThePasswordToContinueTheSignUpProcess);
        }

        [HttpDelete]
        [DawemAuthorize]
        public async Task<ActionResult> DeleteOTPByEmployeeId(int employeeId)
        {
            bool output = await employeeOTPBL.DeleteOTPsByEmployeeNumber(employeeId);
            return Success(output, messageCode: AmgadKeys.DoneOTPDeletedSucessfully);
        }



    }
}