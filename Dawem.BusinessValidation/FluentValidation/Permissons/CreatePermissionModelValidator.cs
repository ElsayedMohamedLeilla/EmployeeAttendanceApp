using Dawem.Enums.Permissions;
using Dawem.Models.Dtos.Permissions.Permissions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Permissons
{
    public class CreatePermissionModelValidator : AbstractValidator<CreatePermissionModel>
    {
        public CreatePermissionModelValidator()
        {
            RuleFor(model => model.ForType).IsInEnum()
                .WithMessage(LeillaKeys.SorryChooseCorrectTypeToApplyPermission);

            RuleFor(model => model.RoleId).GreaterThan(0)
                .When(m => m.ForType == ForRoleOrUser.Role)
                .WithMessage(LeillaKeys.SorryYouMustChooseRoleForPermissionWithTypeRole);

            RuleFor(model => model.RoleId).Null()
                .When(m => m.ForType == ForRoleOrUser.User)
                .WithMessage(LeillaKeys.SorryYouMustNotChooseRoleForPermissionWithTypeUser);

            RuleFor(model => model.UserId).GreaterThan(0)
                .When(m => m.ForType == ForRoleOrUser.User)
                .WithMessage(LeillaKeys.SorryYouMustChooseUserForPermissionWithTypeUser);

            RuleFor(model => model.UserId).Null()
               .When(m => m.ForType == ForRoleOrUser.Role)
               .WithMessage(LeillaKeys.SorryYouMustNotChooseUserForPermissionWithTypeRole);

            RuleFor(model => model.PermissionScreens)
                .Must(model => model != null && model.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseOneScreenAtLeast);

            RuleFor(model => model.PermissionScreens)
                .Must(model => model.GroupBy(s => s.ScreenCode).ToList().All(g => g.Count() == 1))
                .WithMessage(LeillaKeys.SorryYouMustNotDuplicateScreens);

            RuleForEach(x => x.PermissionScreens).SetValidator(new CreatePermissionScreenModelValidator());

        }
    }
    public class CreatePermissionScreenModelValidator : AbstractValidator<PermissionScreenModel>
    {
        public CreatePermissionScreenModelValidator()
        {
            RuleFor(model => model.ScreenCode).IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterCorrectScreenCode);

            RuleFor(model => model.PermissionScreenActions)
                .Must(model => model != null && model.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseOneActionAtLeast);

            RuleFor(model => model.PermissionScreenActions)
               .Must(model => model.GroupBy(s => s.ActionCode).ToList().All(g => g.Count() == 1))
               .WithMessage(LeillaKeys.SorryYouMustNotDuplicateScreenActions);


            RuleForEach(x => x.PermissionScreenActions).SetValidator(new CreatePermissionScreenActionModelValidator());

        }
    }
    public class CreatePermissionScreenActionModelValidator : AbstractValidator<PermissionScreenActionModel>
    {
        public CreatePermissionScreenActionModelValidator()
        {
            RuleFor(model => model.ActionCode).IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterCorrectAction);

        }
    }
}
