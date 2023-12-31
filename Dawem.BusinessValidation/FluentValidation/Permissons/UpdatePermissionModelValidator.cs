using Dawem.Models.Dtos.Permissions.Permissions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Permissons
{
    public class UpdatePermissionModelValidator : AbstractValidator<UpdatePermissionModel>
    {
        public UpdatePermissionModelValidator()
        {
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
