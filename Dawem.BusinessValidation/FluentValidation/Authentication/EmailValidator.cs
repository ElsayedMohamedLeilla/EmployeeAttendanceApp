using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Shared;
using FluentValidation;
namespace Dawem.Validation.FluentValidation.Authentication
{
    class EmailValidator : AbstractValidator<VerifyEmailModel>
    {
        private readonly RequestHeaderContext userContext;


        public EmailValidator(RequestHeaderContext _userContext)
        {
            userContext = _userContext;
            RuleFor(x => x.Email).NotEmpty().WithMessage(TranslationHelper.GetTranslation("EmailRequired", userContext.Lang))
                .EmailAddress().WithMessage(TranslationHelper.GetTranslation("NotValidEmail", userContext.Lang));
        }
    }
}
