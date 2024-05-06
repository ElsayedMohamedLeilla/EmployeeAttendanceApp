using Dawem.Domain.Entities.Dawem;
using Dawem.Models.Dtos.Dawem.Settings;

namespace Dawem.Contract.BusinessValidation.AdminPanel.Settings
{
    public interface ISettingBLValidation
    {
        Task<List<Setting>> UpdateValidation(UpdateSettingModel model);
    }
}
