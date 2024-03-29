using Dawem.Models.Dtos.Dawem.Core.Groups;

namespace Dawem.Contract.BusinessValidation.Dawem.Core
{
    public interface IGroupBLValidation
    {
        Task<bool> CreateValidation(CreateGroupDTO model);
        Task<bool> UpdateValidation(UpdateGroupDTO model);
    }
}
