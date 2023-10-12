using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Response.Provider
{
    public class BranchValidatorResult : BaseResponse
    {
        public bool Result { get; set; }
        public BranchDTO Branch { get; set; }

    }
}
