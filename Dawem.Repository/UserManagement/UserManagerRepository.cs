using Dawem.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dawem.Repository.UserManagement
{
    public class GlameraUserIdentityOptions : IdentityOptions
    {

    }

    public sealed class UserManagerRepository : UserManager<User>
    {

        public UserManagerRepository(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
       IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
       IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
       IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }
        public override Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            ThrowIfDisposed();
            return GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, ResetPasswordTokenPurpose);
        }

        public override async Task<string> GenerateUserTokenAsync(User user, string tokenProvider, string purpose)
        {
            return await base.GenerateUserTokenAsync(user, tokenProvider, purpose);
        }

    }
}
