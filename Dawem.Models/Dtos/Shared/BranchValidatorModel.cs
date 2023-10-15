using Dawem.Enums.General;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Dtos.Shared
{
    public class BranchValidatorModel
    {
        public ChangeType ChangeType { get; set; }
        public BranchDTO Branch { get; set; }
    }
}
