using FluentValidation;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Dtos.Shared;
namespace SmartBusinessERP.BusinessLogic.Validators.FluentValidators
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
