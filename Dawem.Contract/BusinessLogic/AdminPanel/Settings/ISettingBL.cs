using Dawem.Models.Dtos.Dawem.Settings;
using Dawem.Models.Response.AdminPanel.Settings;

namespace Dawem.Contract.BusinessLogic.AdminPanel.Settings
{
    public interface ISettingBL
    {
        Task<bool> Update(UpdateSettingModel model);
        Task<GetSettingsResponse> Get();
    }
}
