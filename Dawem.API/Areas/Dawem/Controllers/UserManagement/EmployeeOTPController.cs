using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Users;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dawem.API.Areas.Dawem.Controllers.UserManagement
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
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
            return Success(await employeeOTPBL.PreSignUp(model), messageCode: AmgadKeys.DoneOTPGeneratedSuccessfullyPleaseUseThisPasswordInTheNextStep);
        }
        

        
    }
}