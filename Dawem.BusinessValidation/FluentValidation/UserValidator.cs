using Dawem.Contract.BusinessLogic.UserManagement;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Shared;
using FluentValidation;
using SmartBusinessERP.Enums;

namespace Dawem.Validation.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUserBL SmartUserBL;
        private readonly RequestHeaderContext userContext;
        public UserValidator(ValidationMode validationMode, IUserBL _smartUserBL, RequestHeaderContext _userContext)
        {
            SmartUserBL = _smartUserBL;
            userContext = _userContext;
            if (validationMode == ValidationMode.Create)
            {


                _ = RuleFor(user => user.MainBranchId).NotEmpty().WithMessage(TranslationHelper.GetTranslation("PleaseChooseBranch", userContext.Lang));
                RuleFor(user => user).Must(args => UniqueEmail(args.Email, args.Id, ValidationMode.Create)).WithMessage(userContext.Lang == "en" ? "Email already exists" : "الايميل مكرر");

                _ = RuleFor(user => user.Email).NotEmpty().WithMessage(TranslationHelper.GetTranslation("EmailRequired", userContext.Lang))
                    .EmailAddress().WithMessage(TranslationHelper.GetTranslation("NotValidEmail", userContext.Lang));
                // _ = RuleFor(p => p.BirthDate).Must(BeAValidAge).WithMessage(TranslationHelper.GetTranslation("BirthNoValid", userContext.Lang));
            }
            if (validationMode == ValidationMode.Update)
            {
                _ = RuleFor(user => user.MainBranchId).NotEmpty().WithMessage(TranslationHelper.GetTranslation("PleaseChooseBranch", userContext.Lang));

                RuleFor(user => user).Must(args => UniqueEmail(args.Email, args.Id, ValidationMode.Update)).WithMessage(userContext.Lang == "en" ? "Email already exists" : "الايميل مكرر");

                _ = RuleFor(user => user.Email).NotEmpty().WithMessage(TranslationHelper.GetTranslation("EmailRequired", userContext.Lang))
                    .EmailAddress().WithMessage(TranslationHelper.GetTranslation("NotValidEmail", userContext.Lang));
                //_ = RuleFor(p => p.BirthDate).Must(BeAValidAge).WithMessage(TranslationHelper.GetTranslation("BirthNoValid", userContext.Lang));

            }


        }

        private bool BeAValidAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now && birthDate >= DateTime.Now.AddYears(-120);
        }
        private bool UniqueEmail(string item, int? Id, ValidationMode validationMode)
        {

            ValidationItems validationItem = new();
            validationItem.Id = Id != null ? Id.Value : null;
            validationItem.Item = item;
            validationItem.validationMode = validationMode;

            var response = SmartUserBL.IsEmailUnique(validationItem);
            return response.Status == ResponseStatus.Success && response.Result;

        }

    }
}
