using Dawem.Enums.Generals;
using Dawem.Models.Dtos.SignalR;
using Dawem.Translations;

namespace Dawem.Helpers
{
    public static class SignalRHelper
    {
        public static TempNotificationModelDTO TempNotificationModelDTO(int newNotificationCount, string lang, NotificationType type, string EmployeeName)
        {
            var notificatioData = new NotificationData()
            {
                EmployeeName = EmployeeName,
                Title = GetNotificationType(type, lang),
                MessageDescription = GetNotificationDescription(type, lang),
            };
            return new TempNotificationModelDTO()
            {
                Data = notificatioData,
                NewCount = newNotificationCount
            };
        }


        private static string GetNotificationType(NotificationType type, string lang)
        {
            return type switch
            {
                NotificationType.NewVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.NewVacationRequest, lang),
                NotificationType.VacationRequest => TranslationHelper.GetTranslation(AmgadKeys.VacationRequest, lang),
                NotificationType.AcceptingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.AcceptingVacationRequest, lang),
                NotificationType.RejectingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingVacationRequest, lang),
                NotificationType.JustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.JustificationRequest, lang),
                NotificationType.AddingInMission => TranslationHelper.GetTranslation(AmgadKeys.AddingInMission, lang),
                NotificationType.PermisionRequest => TranslationHelper.GetTranslation(AmgadKeys.PermisionRequest, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.NewNotification, lang),
            };
        }
        public static string GetNotificationImage(NotificationStatus type)
        {
            switch (type)
            {
                case NotificationStatus.Info:
                    return "/NotificationIcons/info.jpg";
                case NotificationStatus.Error:
                    return "/NotificationIcons/error.jpg";
                case NotificationStatus.Warning:
                    return "/NotificationIcons/warning.jpg";
                default:
                    return "/NotificationIcons/default.jpg";
            }
        }

        private static string GetNotificationPriority(Priority priority, string lang)
        {
            return priority switch
            {
                Priority.High => TranslationHelper.GetTranslation(AmgadKeys.High, lang),
                Priority.Medium => TranslationHelper.GetTranslation(AmgadKeys.Medium, lang),
                Priority.Low => TranslationHelper.GetTranslation(AmgadKeys.Low, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang),
            };
        }

        private static string GetNotificationDescription(NotificationType type, string lang)
        {
            return type switch
            {
                NotificationType.NewVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.AddingNewVacationWaitingApproval, lang),
                NotificationType.AcceptingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.YourVacationIsAccepted, lang),
                NotificationType.RejectingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingVacationRequest, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang),
            };
        }
    }
}
