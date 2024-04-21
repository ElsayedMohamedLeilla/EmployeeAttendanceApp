using Dawem.Domain.Entities.Dawem;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;

namespace Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions
{
    public interface ISettingBLValidation
    {
        Task<List<Setting>> UpdateValidation(UpdateSettingModel model);
    }
}
