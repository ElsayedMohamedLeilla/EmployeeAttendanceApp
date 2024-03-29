using Dawem.Models.Dtos.Dawem.Core.Responsibilities;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IResponsibilityBLValidation
    {
        Task<bool> CreateValidation(CreateResponsibilityModel model);
        Task<bool> UpdateValidation(UpdateResponsibilityModel model);
    }
}
