using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Contract.BusinessLogic.AdminPanel.Settings;
using Dawem.Models.Dtos.Dawem.Settings;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.Settings
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, AdminPanelAuthorize]
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