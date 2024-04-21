using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Subscriptions
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, Authorize, AdminPanelAuthorize]
    public class SettingController : AdminPanelControllerBase
    {
        private readonly ISettingBL settingBL;

        public SettingController(ISettingBL _settingBL)
        {
            settingBL = _settingBL;
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateSettingModel model)
        {
            var result = await settingBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateSettingsSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var settingsResponse = await settingBL.Get();
            return Success(settingsResponse.SettingsGroups);
        }
    }
}