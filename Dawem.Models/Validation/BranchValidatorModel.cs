using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Validation
{
    public class BranchValidatorModel
    {
        public ChangeType ChangeType { get; set; }
        public BranchDTO? Branch { get; set; }
    }
}
