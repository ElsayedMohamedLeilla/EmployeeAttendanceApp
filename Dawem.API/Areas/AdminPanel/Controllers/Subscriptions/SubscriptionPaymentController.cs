using Dawem.API.Areas.Dawem.Controllers;
using Dawem.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.AdminPanel.Controllers.SubscriptionPayments
{
    [Route(LeillaKeys.AdminPanelApiControllerAction), ApiController, Authorize, AdminPanelAuthorize]
    public class SubscriptionPaymentController : AdminPanelControllerBase
    {
        private readonly ISubscriptionPaymentBL subscriptionPaymentBL;

        public SubscriptionPaymentController(ISubscriptionPaymentBL _subscriptionPaymentBL)
        {
            subscriptionPaymentBL = _subscriptionPaymentBL;
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateSubscriptionPaymentModel model)
        {
            var result = await subscriptionPaymentBL.Create(model);
            return Success(result, messageCode: LeillaKeys.DoneCreateSubscriptionPaymentSuccessfully);
        }
        [HttpPut]
        public async Task<ActionResult> Update(UpdateSubscriptionPaymentModel model)
        {
            var result = await subscriptionPaymentBL.Update(model);
            return Success(result, messageCode: LeillaKeys.DoneUpdateSubscriptionPaymentSuccessfully);
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetSubscriptionPaymentsCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var subscriptionPaymentsResponse = await subscriptionPaymentBL.Get(criteria);

            return Success(subscriptionPaymentsResponse.SubscriptionPayments, subscriptionPaymentsResponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetInfo([FromQuery] int subscriptionPaymentId)
        {
            if (subscriptionPaymentId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionPaymentBL.GetInfo(subscriptionPaymentId));
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int subscriptionPaymentId)
        {
            if (subscriptionPaymentId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionPaymentBL.GetById(subscriptionPaymentId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int subscriptionPaymentId)
        {
            if (subscriptionPaymentId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionPaymentBL.Delete(subscriptionPaymentId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int subscriptionPaymentId)
        {
            if (subscriptionPaymentId < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionPaymentBL.Enable(subscriptionPaymentId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await subscriptionPaymentBL.Disable(model));
        }
        [HttpGet]
        public async Task<ActionResult> GetSubscriptionPaymentsInformations()
        {
            return Success(await subscriptionPaymentBL.GetSubscriptionPaymentsInformations());
        }
    }
}