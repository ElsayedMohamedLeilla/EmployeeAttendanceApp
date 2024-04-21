using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.Response.AdminPanel.Subscriptions.Plans;

namespace Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions
{
    public interface ISettingBL
    {
        Task<bool> Update(UpdateSettingModel model);
        Task<GetSettingsResponse> Get();
    }
}
