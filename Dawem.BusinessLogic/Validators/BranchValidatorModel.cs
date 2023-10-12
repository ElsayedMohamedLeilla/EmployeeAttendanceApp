using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Dtos.Provider;

namespace SmartBusinessERP.BusinessLogic.Validators
{
    public class BranchValidatorModel
    {
        public ChangeType ChangeType { get; set; }
        public BranchDTO? Branch { get; set; }
    }
}
