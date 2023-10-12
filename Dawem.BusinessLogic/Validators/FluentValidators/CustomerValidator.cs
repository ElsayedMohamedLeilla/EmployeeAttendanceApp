using FluentValidation;

using SmartBusinessERP.BusinessLogic.Accounting.Contract;

using SmartBusinessERP.Domain.Entities.Accounts;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Dtos.Shared;


namespace SmartBusinessERP.BusinessLogic.Validators.FluentValidators
{
    public  class CustomerValidator : AbstractValidator<CustomerModel>
    {
      
        private readonly RequestHeaderContext userContext;
        private readonly ICustomerBL customerBL;
        public CustomerValidator(ValidationMode validationMode, ICustomerBL _customerBL, RequestHeaderContext _userContext)
        {
            customerBL = _customerBL;
            userContext = _userContext;
            if (validationMode == ValidationMode.Create)
            {
                RuleFor(customer => customer).Must(args => IsNameUnique(args.Name, args.Id, userContext.CompanyId, ValidationMode.Create)).WithMessage(userContext.Lang == "en" ? "Account Number already exists" : "رقم الحساب مكرر");


                RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

                RuleFor(x => x.Type).IsInEnum().WithMessage("Invalid customer type");

                When(x => x.Type == CustomerType.Individual, () =>
                {
                    RuleFor(x => x.CompanyId).NotEmpty().WithMessage("CompanyId is required for Individual customers");
                });


            }
            if (validationMode == ValidationMode.Update)
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

                RuleFor(x => x.Type).IsInEnum().WithMessage("Invalid customer type");

                When(x => x.Type == CustomerType.Individual, () =>
                {
                    RuleFor(x => x.CompanyId).NotEmpty().WithMessage("CompanyId is required for Individual customers");
                });

            }


        }

        private bool IsNameUnique(string item, int? Id, int? CompanyId, ValidationMode validationMode)
        {

            ValidationItems validationItem = new()
            {
                Id = Id != null ? Id.Value : null,
                Item = item,
                validationMode = validationMode,
                CompanyId = CompanyId
            };
            var response = customerBL.IsNameUnique(validationItem);
            return response.Status == ResponseStatus.Success && response.Result;

        }

      

       
    }
}
