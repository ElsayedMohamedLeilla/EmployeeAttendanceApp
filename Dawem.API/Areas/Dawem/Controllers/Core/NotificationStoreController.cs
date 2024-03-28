using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Enums.Generals;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{

    [Route(LeillaKeys.DawemApiControllerAction)]
    [ApiController]
    [Authorize]
    public class NotificationStoreController : BaseController
    {
        private readonly INotificationStoreBL notificationStoreBL;
        private readonly INotificationServiceByFireBaseAdmin notificationService;
        public NotificationStoreController(INotificationStoreBL _NotificationStoreBL, INotificationServiceByFireBaseAdmin _notificationService)
        {
            notificationStoreBL = _NotificationStoreBL;
            notificationService = _notificationService;
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
        [HttpPut]
        public async Task<ActionResult> MarkAsViewed()
        {
            return Success(await notificationStoreBL.MarkAsViewed());
        }
        [HttpGet]
        public async Task<ActionResult> GetNotifications([FromQuery] GetNotificationStoreCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            return Success(await notificationStoreBL.GetNotifications(criteria));
        }

        [HttpGet]
        public async Task<ActionResult> GetUnreadNotificationCount()
        {

            return Success(await notificationStoreBL.GetUnreadNotificationCount());
        }
        [HttpGet]
        public async Task<ActionResult> GetUnViewedNotificationCount()
        {
            return Success(await notificationStoreBL.GetUnViewedNotificationCount());
        }
        [HttpGet]
        public async Task<ActionResult> GetUnreadNotifications([FromQuery] GetNotificationStoreCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            return Success(await notificationStoreBL.GetUnreadNotification(criteria));
        }

        [Route("CreateSendNotification")]
        [HttpPost]
        public async Task<IActionResult> CreateSendNotification([FromQuery] List<int> UserIds, NotificationType notificationType, NotificationStatus notificationStatus)
        {
            if (UserIds.Count == 0)
            {
                return BadRequest();
            }
            var result = await notificationService.Send_Notification_Email(UserIds, notificationType, notificationStatus);
            return Ok(result);
        }


    }
}
