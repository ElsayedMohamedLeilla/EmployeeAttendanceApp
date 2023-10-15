using Dawem.Models.Dtos.Identity;

namespace Dawem.Contract.BusinessValidation
{
    public interface IUserBLValidation
    {
        Task<bool> CreateUserValidation(CreatedUser createdUser);

    }
}
