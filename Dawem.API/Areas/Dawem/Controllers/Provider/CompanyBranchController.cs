using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Models.Criteria.Providers;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Provider
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class CompanyBranchController : AdminPanelControllerBase
    {
        private readonly ICompanyBranchBL companyBranchBL;

        public CompanyBranchController(ICompanyBranchBL _companyBranchBL)
        {
            companyBranchBL = _companyBranchBL;
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetCompanyBranchesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var branchesResponse = await companyBranchBL.GetForDropDown(criteria);

            return Success(branchesResponse.Branches, branchesResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int branchId)
        {
            if (branchId < 1)
            {
                return BadRequest();
            }
            return Success(await companyBranchBL.GetById(branchId));
        }
    }
}