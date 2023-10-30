using Dawem.Models.Dtos.Identity;

namespace Dawem.Contract.BusinessValidation
{
    public interface IUserOldBLValidation
    {
        Task<bool> CreateUserValidation(CreatedUser createdUser);

    }
}
