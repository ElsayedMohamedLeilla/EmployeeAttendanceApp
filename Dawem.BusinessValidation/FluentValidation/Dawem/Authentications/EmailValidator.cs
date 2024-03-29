using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Shared;
using FluentValidation;
namespace Dawem.Validation.FluentValidation.Dawem.Authentications
{
    class EmailValidator : AbstractValidator<VerifyEmailModel>
    {
        private readonly RequestInfo userContext;


        public EmailValidator(RequestInfo _userContext)
        {
            userContext = _userContext;
            RuleFor(x => x.Email).NotEmpty().WithMessage(TranslationHelper.GetTranslation("EmailRequired", userContext.Lang))
                .EmailAddress().WithMessage(TranslationHelper.GetTranslation("NotValidEmail", userContext.Lang));
        }
    }
}
