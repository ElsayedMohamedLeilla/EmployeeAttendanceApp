using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Translations;

namespace Dawem.RealTime.Helper
{
    public static class NotificationHelper
    {



        public static string GetNotificationType(NotificationType type, string lang)
        {
            return type switch
            {
                NotificationType.NewVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.NewVacationRequest, lang),
                NotificationType.AcceptingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.AcceptingVacationRequest, lang),
                NotificationType.RejectingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingVacationRequest, lang),
                NotificationType.NewTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.NewTaskRequest, lang),
                NotificationType.AccecptingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.AccecptingTaskRequest, lang),
                NotificationType.RejectingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingTaskRequest, lang),
                NotificationType.AddingInMission => TranslationHelper.GetTranslation(AmgadKeys.AddingInMission, lang),
                NotificationType.NewPermissionRequent => TranslationHelper.GetTranslation(AmgadKeys.PermisionRequest, lang),
                NotificationType.AcceptingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.AcceptingPermissionRequest, lang),
                NotificationType.RejectingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingPermissionRequest, lang),
                NotificationType.NewJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.JustificationRequest, lang),
                NotificationType.AcceptingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.AcceptingJustificationRequest, lang),
                NotificationType.RejectingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingJustificationRequest, lang),
                NotificationType.NewSummons => TranslationHelper.GetTranslation(AmgadKeys.NewSummons, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.NewNotification, lang),

            };
        }
        public static string GetNotificationImage(NotificationStatus type, IUploadBLC uploadBLC)
        {
            switch (type)
            {
                case NotificationStatus.Info:
                    return uploadBLC.GetFilePath(AmgadKeys.InfoImageName, AmgadKeys.NotificationIcons);
                case NotificationStatus.Error:
                    return uploadBLC.GetFilePath(AmgadKeys.ErrorImageName, AmgadKeys.NotificationIcons);
                case NotificationStatus.Warning:
                    return uploadBLC.GetFilePath(AmgadKeys.WarningImageName, AmgadKeys.NotificationIcons);
                default:
                    return uploadBLC.GetFilePath(AmgadKeys.DefaultImageName, AmgadKeys.NotificationIcons);
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
        public static string GetNotificationDescription(NotificationType type, string lang)
        {
            return type switch
            {
                NotificationType.NewVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.AddingNewVacationWaitingApproval, lang),
                NotificationType.AcceptingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.YourVacationIsAccepted, lang),
                NotificationType.RejectingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingVacationRequest, lang),
                NotificationType.NewTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.NewTaskRequest, lang),
                NotificationType.AccecptingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.AccecptingTaskRequest, lang),
                NotificationType.RejectingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingTaskRequest, lang),
                NotificationType.NewPermissionRequent => TranslationHelper.GetTranslation(AmgadKeys.NewPermisionRequestWaitingApproval, lang),
                NotificationType.AcceptingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.YourPermissionRequestIsAccepted, lang),
                NotificationType.RejectingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.YourPermissionRequestIsRejected, lang),
                NotificationType.NewJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.JustificationRequest, lang),
                NotificationType.AcceptingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.YourJustificationRequestIsAccepted, lang),
                NotificationType.RejectingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.YourJustificationRequestIsRejected, lang),
                NotificationType.AddingInMission => TranslationHelper.GetTranslation(AmgadKeys.AddingInMission, lang),
                NotificationType.NewSummons => TranslationHelper.GetTranslation(AmgadKeys.NewSummons, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang),
            };
        }


    }
}
