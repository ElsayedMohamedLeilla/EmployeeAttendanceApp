using Dawem.Contract.BusinessLogic.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Controllers.Core
{

    [Route(LeillaKeys.ApiControllerAction)]
    [ApiController]
    [Authorize]
    public class NotificationStoreController : BaseController
    {
        private readonly INotificationStoreBL notificationStoreBL;
        public NotificationStoreController(INotificationStoreBL _NotificationStoreBL)
        {
            notificationStoreBL = _NotificationStoreBL;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetNotificationStoreCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var result = await notificationStoreBL.Get(criteria);
            return Success(result.NotificationStores, result.TotalCount);
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int NotificationStoreId)
        {
            if (NotificationStoreId < 1)
            {
                return BadRequest();
            }
            return Success(await notificationStoreBL.Delete(NotificationStoreId));
        }
        [HttpPut]
        public async Task<ActionResult> Enable(int NotificationStoreId)
        {
            if (NotificationStoreId < 1)
            {
                return BadRequest();
            }
            return Success(await notificationStoreBL.Enable(NotificationStoreId));
        }
        [HttpPut]
        public async Task<ActionResult> Disable([FromQuery] DisableModelDTO model)
        {
            if (model.Id < 1)
            {
                return BadRequest();
            }
            return Success(await notificationStoreBL.Disable(model));
        }
        [HttpPut]
        public async Task<ActionResult> MarkAsRead(int notificationStoreId)
        {
            if (notificationStoreId < 1)
            {
                return BadRequest();
            }
            return Success(await notificationStoreBL.MarkAsRead(notificationStoreId));
        }
        [HttpGet]
        public async Task<ActionResult> GetNotificationsByUserId()
        {
            return Success(await notificationStoreBL.GetNotificationsByUserId());
        }
        [HttpGet]
        public async Task<ActionResult> GetUnreadNotificationCount()
        {
           
            return Success(await notificationStoreBL.GetUnreadNotificationCount());
        }

        
    }
}
