using Dawem.Contract.BusinessLogic.Subscriptions;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Subscriptions;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Subscriptions
{
    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionBL subscriptionBL;

        public SubscriptionController(ISubscriptionBL _subscriptionBL)
        {
            subscriptionBL = _subscriptionBL;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateSubscriptionModel model)
        {
            var result = await subscriptionBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateSubscriptionSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateSubscriptionModel model)
        {

            var result = await subscriptionBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateSubscriptionSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSubscriptionsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var departmensresponse = await subscriptionBL.Get(criteria);

            return Success(departmensresponse.Subscriptions, departmensresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int subscriptionId)
        {
            if (subscriptionId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionBL.GetInfo(subscriptionId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int subscriptionId)
        {
            if (subscriptionId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionBL.GetById(subscriptionId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int subscriptionId)
        {
            if (subscriptionId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionBL.Delete(subscriptionId));
        }

        [HttpPut]
        public async Task<ActionResult> Enable(int subscriptionId)
        {
            if (subscriptionId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionBL.Enable(subscriptionId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetSubscriptionsInformations()
        {
            return Success(await subscriptionBL.GetSubscriptionsInformations());
        }
    }
}