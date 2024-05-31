using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Subscriptions
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, AdminPanelAuthorize]
    public class ScreenController : AdminPanelControllerBase
    {
        private readonly IScreenBL screenBL;
        private readonly IScreenBLC screenBLC;
        private readonly RequestInfo requestInfo;

        public ScreenController(IScreenBL _screenBL, IScreenBLC _screenBLC, RequestInfo requestInfo)
        {
            screenBL = _screenBL;
            screenBLC = _screenBLC;
            this.requestInfo = requestInfo;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateScreenModel model)
        {
            var result = await screenBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateScreenSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateScreenModel model)
        {

            var result = await screenBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateScreenSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetScreensCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var screensResponse = await screenBL.Get(criteria);

            return Success(screensResponse.Screens, screensResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetScreensCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var screensResponse = await screenBL.GetForDropDown(criteria);

            return Success(screensResponse.Screens, screensResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int screenId)
        {
            if (screenId < 1)
            {
                return BadRequest();
            }
            return Success(await screenBL.GetInfo(screenId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int screenId)
        {
            if (screenId < 1)
            {
                return BadRequest();
            }
            return Success(await screenBL.GetById(screenId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int screenId)
        {
            if (screenId < 1)
            {
                return BadRequest();
            }
            return Success(await screenBL.Delete(screenId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int screenId)
        {
            if (screenId < 1)
            {
                return BadRequest();
            }
            return Success(await screenBL.Enable(screenId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await screenBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetScreensInformations()
        {
            return Success(await screenBL.GetScreensInformations());
        }
        [HttpGet]
        public async Task<ActionResult> GetAllScreensWithAvailableActions()
        {
            return Success(await screenBLC.GetAllScreensWithAvailableActions());
        }
    }
}