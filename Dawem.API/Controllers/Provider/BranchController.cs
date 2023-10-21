using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Provider;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Provider
{
    [Route(DawemKeys.ApiControllerAction)]
    [ApiController]
    public class BranchController : BaseController
    {
        private readonly IBranchBL branchBL;

        public BranchController(IBranchBL _branchBL)
        {
            branchBL = _branchBL;
        }


        [HttpPost]
        public async Task<ActionResult> Get(GetBranchesCriteria criteria)
        {
            var result = await branchBL.Get(criteria);
            return Success(result, result.TotalCount);
        }

        [HttpPost]
        public async Task<ActionResult> GetInfo(GetBranchInfoCriteria criteria)
        {
            var result = await branchBL.GetInfo(criteria);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BranchDTO branchDTO)
        {
            var result = await branchBL.Create(branchDTO);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> Update(BranchDTO branch)
        {
            return Success(await branchBL.Update(branch));
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] int Id)
        {
            return Success(await branchBL.Delete(Id));
        }
    }
}