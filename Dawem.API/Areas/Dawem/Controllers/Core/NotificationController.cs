using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Enums.Generals;
using Dawem.Models.Criteria.Core;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;

namespace Dawem.API.Areas.Dawem.Controllers.Core
{
    [Route(LeillaKeys.DawemApiControllerAction), ApiController, DawemAuthorize]
    public class NotificationController : DawemControllerBase
    {
        private readonly INotificationBL notificationBL;
        private readonly INotificationService notificationService;
        public NotificationController(INotificationBL _notificationBL, INotificationService _notificationService)
        {
            notificationBL = _notificationBL;
            notificationService = _notificationService;
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetNotificationCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            return Success(await notificationBL.Get(criteria));
        }
        [HttpPut]
        public async Task<ActionResult> MarkAsRead(int notificationStoreId)
        {
            if (notificationStoreId < 1)
            {
                return BadRequest();
            }
            return Success(await notificationBL.MarkAsRead(notificationStoreId));
        }
        [HttpPut]
        public async Task<ActionResult> MarkAsViewed()
        {
            return Success(await notificationBL.MarkAsViewed());
        }
        [HttpGet]
        public async Task<ActionResult> GetUnreadNotificationCount()
        {

            return Success(await notificationBL.GetUnreadNotificationCount());
        }
        [HttpGet]
        public async Task<ActionResult> GetUnViewedNotificationCount()
        {
            return Success(await notificationBL.GetUnViewedNotificationCount());
        }
        [Route("CreateSendNotification")]
        [HttpPost]
        public async Task<IActionResult> CreateSendNotification([FromQuery] SendNotificationsAndEmailsModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await notificationService.SendNotificationsAndEmails(model);
            return Ok(result);
        }
    }
}
