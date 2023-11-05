using Dawem.Contract.BusinessLogic.Core;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{
    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize(Roles = LeillaKeys.FullAccess)]
    public class GroupController : BaseController
    {
        private readonly IGroupBL groupBL;

        public GroupController(IGroupBL _groupBL)
        {
            groupBL = _groupBL;
        }

        [HttpPost]
        public async Task<ActionResult> Get(GetGroupsCriteria criteria)
        {
            return Success(await groupBL.Get(criteria));
        }

        [HttpPost]
        public async Task<ActionResult> GetInfo(GetGroupInfoCriteria criteria)
        {
            return Success(await groupBL.GetInfo(criteria));
        }



        [HttpPost]
        public async Task<ActionResult> GetById([FromBody] int id)
        {
            return Success(await groupBL.GetById(id));
        }


        [HttpPost]
        public async Task<ActionResult> Create(Group Group)
        {
            return Success(await groupBL.Create(Group));
        }

        [HttpPost]
        public async Task<ActionResult> Update(Group Group)
        {
            var result = await groupBL.Update(Group);
            return Success(result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] int Id)
        {
            var result = await groupBL.Delete(Id);
            return Success(result);
        }

    }
}
