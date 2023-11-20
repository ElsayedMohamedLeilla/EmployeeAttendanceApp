using Dawem.Models.Dtos.Core.Group;

namespace Dawem.Contract.BusinessValidation.Core
{
    public interface IGroupBLValidation
    {
        Task<bool> CreateValidation(CreateGroupDTO model);
        Task<bool> UpdateValidation(UpdateGroupDTO model);
    }
}
