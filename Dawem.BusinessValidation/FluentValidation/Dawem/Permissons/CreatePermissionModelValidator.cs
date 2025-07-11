﻿using Dawem.Enums.Permissions;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Permissons
{
    public class CreatePermissionModelValidator : AbstractValidator<CreatePermissionModel>
    {
        public CreatePermissionModelValidator()
        {
            RuleFor(model => model.ForType).IsInEnum()
                .WithMessage(LeillaKeys.SorryChooseCorrectTypeToApplyPermission);

            RuleFor(model => model.ResponsibilityId).GreaterThan(0)
                .When(m => m.ForType == ForResponsibilityOrUser.Responsibility)
                .WithMessage(LeillaKeys.SorryYouMustChooseResponsibilityForPermissionWithTypeResponsibility);

            RuleFor(model => model.ResponsibilityId).Null()
                .When(m => m.ForType == ForResponsibilityOrUser.User)
                .WithMessage(LeillaKeys.SorryYouMustNotChooseResponsibilityForPermissionWithTypeUser);

            RuleFor(model => model.UserId).GreaterThan(0)
                .When(m => m.ForType == ForResponsibilityOrUser.User)
                .WithMessage(LeillaKeys.SorryYouMustChooseUserForPermissionWithTypeUser);

            RuleFor(model => model.UserId).Null()
               .When(m => m.ForType == ForResponsibilityOrUser.Responsibility)
               .WithMessage(LeillaKeys.SorryYouMustNotChooseUserForPermissionWithTypeResponsibility);

            RuleFor(model => model.Screens)
                .Must(model => model != null && model.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseOneScreenAtLeast);

            RuleFor(model => model.Screens)
                .Must(model => model.All(s => s.ScreenId > 0))
                .WithMessage(LeillaKeys.SorryYouMustEnterScreenId);

            /*RuleFor(model => model.Screens)
                .Must(model => model.GroupBy(s => s.ScreenCode).ToList().All(g => g.Count() == 1))
                .WithMessage(LeillaKeys.SorryYouMustNotDuplicateScreens);*/

            RuleFor(model => model.Screens)
                .Must(model => model.GroupBy(s => s.ScreenId).ToList().All(g => g.Count() == 1))
                .WithMessage(LeillaKeys.SorryYouMustNotDuplicateScreens);

            RuleForEach(x => x.Screens).SetValidator(new CreatePermissionScreenModelValidator());

        }
    }
    public class CreatePermissionScreenModelValidator : AbstractValidator<PermissionScreenModel>
    {
        public CreatePermissionScreenModelValidator()
        {
            /*RuleFor(model => model.ScreenCode).
                Must(screenCode => screenCode >= 0).
                WithMessage(LeillaKeys.SorryYouMustEnterCorrectScreenCode);*/

            RuleFor(model => model.ScreenId).
                Must(screenId => screenId >= 0).
                WithMessage(LeillaKeys.SorryYouMustEnterCorrectScreenId);

            RuleFor(model => model.Actions)
                .Must(model => model != null && model.Count > 0)
                .WithMessage(LeillaKeys.SorryYouMustChooseOneActionAtLeast);

            RuleFor(model => model.Actions)
               .Must(model => model.GroupBy(actionCode => actionCode).ToList().All(g => g.Count() == 1))
               .WithMessage(LeillaKeys.SorryYouMustNotDuplicateScreenActions);

            RuleFor(model => model.Actions)
               .Must(model => model.Any(actionCode => actionCode == ApplicationActionCode.ViewingAction))
               .WithMessage(LeillaKeys.SorryYouMustSelectViewingActionWhenSelectAnyActionForAnyScreen);

            RuleForEach(x => x.Actions).SetValidator(new CreatePermissionScreenActionModelValidator());

        }
    }
    public class CreatePermissionScreenActionModelValidator : AbstractValidator<ApplicationActionCode>
    {
        public CreatePermissionScreenActionModelValidator()
        {
            RuleFor(actionCode => actionCode).IsInEnum()
                .WithMessage(LeillaKeys.SorryYouMustEnterCorrectAction);

        }
    }
}
