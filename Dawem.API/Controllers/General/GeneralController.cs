using Dawem.Contract.BusinessLogic.Employees.Department;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.General
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize(Roles =LeillaKeys.NURSE)]
    public class GeneralController : BaseController
    {
        private readonly IGeneralBL generalBL;
        public GeneralController(IGeneralBL _generalBL)
        {
            generalBL = _generalBL;
        }
        [HttpGet]
        public ActionResult GetWeekDays()
        {
            var weekDays = generalBL.GetWeekDays();
            return Success(weekDays, weekDays.Count);
        }
    }
}