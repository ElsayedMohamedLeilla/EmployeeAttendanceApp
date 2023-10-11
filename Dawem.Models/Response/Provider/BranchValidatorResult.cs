using SmartBusinessERP.Models.Dtos.Provider;

namespace SmartBusinessERP.Models.Response.Provider
{
    public class BranchValidatorResult : BaseResponse
    {
        public bool Result { get; set; }
        public BranchDTO Branch { get; set; }

    }
}
