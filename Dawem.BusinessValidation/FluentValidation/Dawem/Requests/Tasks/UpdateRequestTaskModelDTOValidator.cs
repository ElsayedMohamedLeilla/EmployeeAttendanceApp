﻿using Dawem.Models.Requests.Tasks;
using Dawem.Translations;
using FluentValidation;

namespace Dawem.Validation.FluentValidation.Dawem.Requests.Tasks
{
    public class UpdateRequestTaskModelDTOValidator : AbstractValidator<UpdateRequestTaskModelDTO>
    {
        public UpdateRequestTaskModelDTOValidator()
        {
            RuleFor(model => model.Id).GreaterThan(0).
                    WithMessage(LeillaKeys.SorryYouMustEnterRequestId);

            RuleFor(model => model.EmployeeId).NotNull()
                .When(m => m.ForEmployee)
                .WithMessage(LeillaKeys.SorryYouMustChooseEmployeeForRequestTask);

            RuleFor(model => model.TaskTypeId).GreaterThan(0)
               .WithMessage(LeillaKeys.SorryYouMustChooseTaskType);

            RuleFor(model => model.DateFrom).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateFromForTaskRequest);

            RuleFor(model => model.DateTo).Must(d => d != default)
               .WithMessage(LeillaKeys.SorryYouMustEnterDateToForTaskRequest);

            RuleFor(model => model).Must(d => d.DateTo >= d.DateFrom)
              .WithMessage(LeillaKeys.SorryDateToMustGreaterThanOrEqualDateFrom);

            RuleFor(model => model.TaskEmployeeIds)
                .NotNull()
               .WithMessage(LeillaKeys.SorryYouMustChooseTeamForTaskRequest)
                .Must(t => t.Count > 0)
               .WithMessage(LeillaKeys.SorryYouMustChooseTeamForTaskRequest)
                .Must(t => t.All(t => t > 0))
               .WithMessage(LeillaKeys.SorryYouMustChooseTeamForTaskRequest);

        }
    }
}
