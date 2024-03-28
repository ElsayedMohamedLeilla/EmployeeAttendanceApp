using Dawem.Enums.Permissions;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Permissons
{
    public class UpdatePermissionModelValidator : AbstractValidator<UpdatePermissionModel>
    {
        public UpdatePermissionModelValidator()
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

            RuleFor(model => model.Id).GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustEnterPermissionId);

            RuleFor(model => model.RoleId).GreaterThan(0)
                .WithMessage(LeillaKeys.SorryYouMustChooseRoleForPermission);

            RuleFor(model => model.PermissionScreens)
                .Must(model => model != null && model.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseOneScreenAtLeast);

            RuleForEach(x => x.PermissionScreens).SetValidator(new UpdatePermissionScreenModelValidator());

        }
    }
    public class UpdatePermissionScreenModelValidator : AbstractValidator<PermissionScreenModel>
    {
        public UpdatePermissionScreenModelValidator()
        {
            RuleFor(model => model.ScreenCode).IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterCorrectScreenCode);

            RuleFor(model => model.PermissionScreenActions)
                .Must(model => model != null && model.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseOneActionAtLeast);

            RuleFor(model => model.PermissionScreenActions)
               .Must(model => model.GroupBy(s => s.ActionCode).ToList().All(g => g.Count() == 1))
               .WithMessage(LeillaKeys.SorryYouMustNotDuplicateScreenActions);

            RuleFor(model => model.PermissionScreenActions)
               .Must(model => model.Any(a => a.ActionCode == ApplicationAction.ViewingAction))
               .WithMessage(LeillaKeys.SorryYouMustSelectViewingActionWhenSelectAnyActionForAnyScreen);

            RuleForEach(x => x.PermissionScreenActions).SetValidator(new UpdatePermissionScreenActionModelValidator());

        }
    }
    public class UpdatePermissionScreenActionModelValidator : AbstractValidator<PermissionScreenActionModel>
    {
        public UpdatePermissionScreenActionModelValidator()
        {
            RuleFor(model => model.ActionCode).IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterCorrectAction);

        }
    }
}
