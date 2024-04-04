using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.Groups;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction), ApiController, Authorize, DawemAuthorize]
    
    
    public class GroupController : DawemControllerBase
    {
        private readonly IGroupBL groupBL;
        public GroupController(IGroupBL _GroupBL)
        {
            groupBL = _GroupBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateGroupDTO model)
        {
            var result = await groupBL.Create(model);
            return Success(result, messageCode: AmgadKeys.DoneCreateGroupSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateGroupDTO model)
        {

            var result = await groupBL.Update(model);
            return Success(result, messageCode: AmgadKeys.DoneUpdateGroupSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetGroupCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await groupBL.Get(criteria);
            return Success(result.Groups, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetGroupCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await groupBL.GetForDropDown(criteria);
            return Success(result.Groups, result.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int groupId)
        {
            if (groupId < 1)
            {
                return BadRequest();
            }
            return Success(await groupBL.GetInfo(groupId));
        }

        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int groupId)
        {
            if (groupId < 1)
            {
                return BadRequest();
            }
            return Success(await groupBL.GetById(groupId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int groupId)
        {
            if (groupId < 1)
            {
                return BadRequest();
            }
            return Success(await groupBL.Delete(groupId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int GroupId)
        {
            if (GroupId < 1)
            {
                return BadRequest();
            }
            return Success(await groupBL.Enable(GroupId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await groupBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetGroupsInformations()
        {
            return Success(await groupBL.GetGroupsInformations());
        }
    }
}