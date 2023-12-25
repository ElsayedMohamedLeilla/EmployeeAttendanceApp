using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Providers;

namespace Dawem.Models.Dtos.Shared
{
    public class BranchValidatorModel
    {
        public ChangeType ChangeType { get; set; }
        public BranchDTO Branch { get; set; }
    }
}
