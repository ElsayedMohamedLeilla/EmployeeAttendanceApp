using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Subscriptions
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, AdminPanelAuthorize]
    public class ScreenGroupController : AdminPanelControllerBase
    {
        private readonly IScreenGroupBL screenGroupBL;
        private readonly RequestInfo requestInfo;

        public ScreenGroupController(IScreenGroupBL _screenGroupBL, RequestInfo requestInfo)
        {
            screenGroupBL = _screenGroupBL;
            this.requestInfo = requestInfo;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateScreenGroupModel model)
        {
            var result = await screenGroupBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateScreenGroupSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateScreenGroupModel model)
        {

            var result = await screenGroupBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateScreenGroupSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetScreensCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var screenGroupsResponse = await screenGroupBL.Get(criteria);

            return Success(screenGroupsResponse.ScreenGroups, screenGroupsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetScreensCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var screenGroupsResponse = await screenGroupBL.GetForDropDown(criteria);

            return Success(screenGroupsResponse.ScreenGroups, screenGroupsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int screenGroupId)
        {
            if (screenGroupId < 1)
            {
                return BadRequest();
            }
            return Success(await screenGroupBL.GetInfo(screenGroupId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int screenGroupId)
        {
            if (screenGroupId < 1)
            {
                return BadRequest();
            }
            return Success(await screenGroupBL.GetById(screenGroupId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int screenGroupId)
        {
            if (screenGroupId < 1)
            {
                return BadRequest();
            }
            return Success(await screenGroupBL.Delete(screenGroupId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int screenGroupId)
        {
            if (screenGroupId < 1)
            {
                return BadRequest();
            }
            return Success(await screenGroupBL.Enable(screenGroupId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await screenGroupBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetScreenGroupsInformations()
        {
            return Success(await screenGroupBL.GetScreenGroupsInformations());
        }
    }
}