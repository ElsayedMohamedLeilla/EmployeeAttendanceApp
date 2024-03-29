using Dawem.Contract.BusinessLogic.Dawem.General;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.General
{
    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
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