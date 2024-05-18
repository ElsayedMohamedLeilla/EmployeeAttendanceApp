using Dawem.Models.DTOs.Dawem.RealTime.Firebase;

namespace Dawem.Models.Criteria.Core
{
    public class NotificationUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<NotificationUserTokenModel> UserTokens { get; set; }
    }
}
