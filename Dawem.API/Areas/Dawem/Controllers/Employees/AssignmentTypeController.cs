using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Models.Dtos.Dawem.Employees.AssignmentTypes;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Employees
{
    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
    public class AssignmentTypeController : BaseController
    {
        private readonly IAssignmentTypeBL assignmentTypeBL;

        public AssignmentTypeController(IAssignmentTypeBL _assignmentTypeBL)
        {
            assignmentTypeBL = _assignmentTypeBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAssignmentTypeModel model)
        {
            var result = await assignmentTypeBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateAssignmentTypeSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateAssignmentTypeModel model)
        {

            var result = await assignmentTypeBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateAssignmentTypeSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetAssignmentTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await assignmentTypeBL.Get(criteria);

            return Success(departmensresponse.AssignmentTypes, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetForDropDown([FromQuery] GetAssignmentTypesCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await assignmentTypeBL.GetForDropDown(criteria);

            return Success(departmensresponse.AssignmentTypes, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int assignmentTypeId)
        {
            if (assignmentTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await assignmentTypeBL.GetInfo(assignmentTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int assignmentTypeId)
        {
            if (assignmentTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await assignmentTypeBL.GetById(assignmentTypeId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int assignmentTypeId)
        {
            if (assignmentTypeId < 1)
            {
                return BadRequest();
            }
            return Success(await assignmentTypeBL.Delete(assignmentTypeId));
        }
        [HttpGet]
        public async Task<ActionResult> GetAssignmentTypesInformations()
        {
            return Success(await assignmentTypeBL.GetAssignmentTypesInformations());
        }

    }
}