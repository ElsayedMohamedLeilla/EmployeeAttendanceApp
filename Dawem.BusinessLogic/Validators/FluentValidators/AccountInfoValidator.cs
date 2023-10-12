using FluentValidation;

using SmartBusinessERP.BusinessLogic.Accounting.Contract;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Dtos.Accounting;
using SmartBusinessERP.Models.Dtos.Shared;


namespace SmartBusinessERP.BusinessLogic.Validators.FluentValidators
{
    public  class AccountInfoValidator : AbstractValidator<AccountInfoModel>
    {
      
        private readonly RequestHeaderContext userContext;
        private readonly IAccountInfoBL accountInfoBL;
        public AccountInfoValidator(ValidationMode validationMode, IAccountInfoBL _accountInfoBL , RequestHeaderContext _userContext)
        {
            accountInfoBL = _accountInfoBL;
            userContext = _userContext;
            if (validationMode == ValidationMode.Create)
            {
                RuleFor(AccountInfo => AccountInfo).Must(args => UniqueCode(args.AccountNo, args.Id, userContext.CompanyId , ValidationMode.Create)).WithMessage(userContext.Lang == "en" ? "Account Number already exists" : "رقم الحساب مكرر");
             
               
                RuleFor(AccountInfo => AccountInfo.AccountNo).NotEmpty().WithMessage(userContext.Lang == "en" ? "Please enter AccountNo" : "من فضلك أدخل رقم الحساب" ); 
                RuleFor(AccountInfo => AccountInfo.AccountTypeId).Must(a=>a > 0).WithMessage(userContext.Lang == "en" ? "Please enter AccountType" : "من فضلك أختر نوع الحساب" );
                //RuleFor(AccountInfo => AccountInfo.AccountGroupId).NotEmpty().WithMessage("Please Choose AccountGroup").When(a=>a.IsDebit != null);
                RuleFor(AccountInfo => AccountInfo.AccountNo).Length(1).When(a=>a.LevelNumber == 1 ).WithMessage(userContext.Lang == "en" ? "Length should be 1 digit" : "رقم الحساب لابد أن يتكون من 1 أرقام");
                RuleFor(AccountInfo => AccountInfo.AccountNo).Length(3).When(a => a.LevelNumber == 2 ).WithMessage(userContext.Lang == "en" ? "Length should be 2 digit" : "رقم الحساب لابد أن يتكون من 3 أرقام");
                RuleFor(AccountInfo => AccountInfo).Must(a=>a.ParentId != null).When(a => a.IsChild == true).WithMessage(userContext.Lang == "en" ? "Child Account must have parent " : "الحساب الفرعي لابد أن يندرج تحت حساب");
             

            }
            if (validationMode == ValidationMode.Update)
            {
                RuleFor(AccountInfo => AccountInfo).Must(args => UniqueCode(args.AccountNo, args.Id, userContext.CompanyId, ValidationMode.Update)).WithMessage(userContext.Lang == "en" ? "Account Number already exists" : "رقم الحساب مكرر");
                RuleFor(AccountInfo => AccountInfo.Id).NotEqual(0).WithMessage("Id Is NULL!!");
                RuleFor(AccountInfo => AccountInfo.AccountNo).NotEmpty().WithMessage(userContext.Lang == "en" ? "Please enter AccountNo" : "من فضلك أدخل رقم الحساب");
                RuleFor(AccountInfo => AccountInfo.AccountTypeId).NotEmpty().WithMessage(userContext.Lang == "en" ? "Please enter AccountType" : "من فضلك أختر نوع الحساب");
                //RuleFor(AccountInfo => AccountInfo.AccountGroupId).NotEmpty().WithMessage("Please Choose AccountGroup").When(a=>a.IsDebit != null);
                RuleFor(AccountInfo => AccountInfo.AccountNo).Length(1).When(a => a.LevelNumber == 1 ).WithMessage(userContext.Lang == "en" ? "Length should be 1 digit" : "رقم الحساب لابد أن يتكون من 1 أرقام");
                RuleFor(AccountInfo => AccountInfo.AccountNo).Length(3).When(a => a.LevelNumber == 2 ).WithMessage(userContext.Lang == "en" ? "Length should be 3 digit" : "رقم الحساب لابد أن يتكون من 3 أرقام");
                //RuleFor(AccountInfo => AccountInfo.AccountNo).Length(6).When(a => a.LevelNumber == 3 ).WithMessage(requestParam.Lang == "en" ? "Length should be 6 digit" : "رقم الحساب لابد أن يتكون من 6 أرقام");

                //RuleFor(AccountInfo => AccountInfo.AccountGroupId).NotEmpty().WithMessage("Please Choose AccountGroup");

            }


        }

        private bool UniqueCode(string item, int? Id, int? CompanyId, ValidationMode validationMode)
        {

            ValidationItems validationItem = new()
            {
                Id = Id != null ? Id.Value : null,
                Item = item,
                validationMode = validationMode,
                CompanyId = CompanyId
            };
            var response = accountInfoBL.IsNumberUnique(validationItem);
            return response.Status == ResponseStatus.Success && response.Result;

        }

      

       
    }
}
