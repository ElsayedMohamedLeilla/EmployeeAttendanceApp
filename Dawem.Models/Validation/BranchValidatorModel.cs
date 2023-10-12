using Dawem.Enums.General;
using Dawem.Models.Dtos.Provider;

namespace SmartBusinessERP.BusinessLogic.Validators
{
    public class BranchValidatorModel
    {
        public ChangeType ChangeType { get; set; }
        public BranchDTO? Branch { get; set; }
    }
}
