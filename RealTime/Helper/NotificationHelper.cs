using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
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
                NotificationType.AcceptingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.AccecptingTaskRequest, lang),
                NotificationType.RejectingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingTaskRequest, lang),
                NotificationType.AddingInMission => TranslationHelper.GetTranslation(AmgadKeys.AddingInMission, lang),
                NotificationType.NewPermissionRequent => TranslationHelper.GetTranslation(AmgadKeys.PermisionRequest, lang),
                NotificationType.AcceptingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.AcceptingPermissionRequest, lang),
                NotificationType.RejectingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingPermissionRequest, lang),
                NotificationType.NewJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.JustificationRequest, lang),
                NotificationType.AcceptingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.AcceptingJustificationRequest, lang),
                NotificationType.RejectingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingJustificationRequest, lang),
                NotificationType.NewAssignmentRequest => TranslationHelper.GetTranslation(AmgadKeys.AssignmentRequest, lang),
                NotificationType.AcceptingAssignmentRequest => TranslationHelper.GetTranslation(AmgadKeys.AcceptingAssignmentRequest, lang),
                NotificationType.RejectingAssignmentRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingAssignmentRequest, lang),
                NotificationType.NewSummon => TranslationHelper.GetTranslation(AmgadKeys.NewSummon, lang),
                NotificationType.SummonMissed => TranslationHelper.GetTranslation(LeillaKeys.YouHaveMissedTheSummon, lang),
                NotificationType.NewChangeInSchedule => TranslationHelper.GetTranslation(LeillaKeys.NewChangeInYourSchedule, lang),
                NotificationType.DoNotForgetSummon => TranslationHelper.GetTranslation(LeillaKeys.DoNotForgetYourSummonFingerprint, lang),
                NotificationType.DoNotForgetSignIn => TranslationHelper.GetTranslation(LeillaKeys.DoNotForgetSignIn, lang),
                NotificationType.ForgetSignIn => TranslationHelper.GetTranslation(LeillaKeys.ForgetSignIn, lang),
                NotificationType.DoNotForgetSignOut => TranslationHelper.GetTranslation(LeillaKeys.DoNotForgetSignOut, lang),
                NotificationType.ForgetSignOut => TranslationHelper.GetTranslation(LeillaKeys.ForgetSignOut, lang),
                NotificationType.NewOvertimeRequest => TranslationHelper.GetTranslation(LeillaKeys.NewOvertimeRequest, lang),
                NotificationType.AcceptingOvertimeRequest => TranslationHelper.GetTranslation(LeillaKeys.AcceptingOvertimeRequest, lang),
                NotificationType.RejectingOvertimeRequest => TranslationHelper.GetTranslation(LeillaKeys.RejectingOvertimeRequest, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.NewNotification, lang),

            };
        }
        public static DawemAdminApplicationScreenCode GetNotificationScreenCode(NotificationType type)
        {
            return type switch
            {
                NotificationType.NewVacationRequest => DawemAdminApplicationScreenCode.RequestVacation,
                NotificationType.AcceptingVacationRequest => DawemAdminApplicationScreenCode.RequestVacation,
                NotificationType.RejectingVacationRequest => DawemAdminApplicationScreenCode.RequestVacation,
                NotificationType.NewTaskRequest => DawemAdminApplicationScreenCode.RequestTask,
                NotificationType.AcceptingTaskRequest => DawemAdminApplicationScreenCode.RequestTask,
                NotificationType.RejectingTaskRequest => DawemAdminApplicationScreenCode.RequestTask,
                NotificationType.AddingInMission => DawemAdminApplicationScreenCode.RequestTask,
                NotificationType.NewPermissionRequent => DawemAdminApplicationScreenCode.RequestPermission,
                NotificationType.AcceptingPermissionRequest => DawemAdminApplicationScreenCode.RequestPermission,
                NotificationType.RejectingPermissionRequest => DawemAdminApplicationScreenCode.RequestPermission,
                NotificationType.NewJustificationRequest => DawemAdminApplicationScreenCode.RequestJustification,
                NotificationType.AcceptingJustificationRequest => DawemAdminApplicationScreenCode.RequestJustification,
                NotificationType.RejectingJustificationRequest => DawemAdminApplicationScreenCode.RequestJustification,
                NotificationType.NewAssignmentRequest => DawemAdminApplicationScreenCode.RequestAssignment,
                NotificationType.AcceptingAssignmentRequest => DawemAdminApplicationScreenCode.RequestAssignment,
                NotificationType.RejectingAssignmentRequest => DawemAdminApplicationScreenCode.RequestAssignment,
                NotificationType.NewSummon => DawemAdminApplicationScreenCode.Summon,
                NotificationType.SummonMissed => DawemAdminApplicationScreenCode.Summon,
                NotificationType.NewChangeInSchedule => DawemAdminApplicationScreenCode.Schedule,
                NotificationType.DoNotForgetSummon => DawemAdminApplicationScreenCode.Summon,
                NotificationType.DoNotForgetSignIn => DawemAdminApplicationScreenCode.Schedule,
                NotificationType.ForgetSignIn => DawemAdminApplicationScreenCode.Schedule,
                NotificationType.DoNotForgetSignOut => DawemAdminApplicationScreenCode.Schedule,
                NotificationType.ForgetSignOut => DawemAdminApplicationScreenCode.Schedule,
                NotificationType.NewOvertimeRequest => DawemAdminApplicationScreenCode.RequestOvertime,
                NotificationType.AcceptingOvertimeRequest => DawemAdminApplicationScreenCode.RequestOvertime,
                NotificationType.RejectingOvertimeRequest => DawemAdminApplicationScreenCode.RequestOvertime,
                _ => DawemAdminApplicationScreenCode.Dashboard

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
        private static string GetNotificationPriority(NotificationPriority priority, string lang)
        {
            return priority switch
            {
                NotificationPriority.High => TranslationHelper.GetTranslation(AmgadKeys.High, lang),
                NotificationPriority.Medium => TranslationHelper.GetTranslation(AmgadKeys.Medium, lang),
                NotificationPriority.Low => TranslationHelper.GetTranslation(AmgadKeys.Low, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang),
            };
        }
        public static string GetNotificationDescription(NotificationType type, string lang)
        {
            return type switch
            {
                NotificationType.NewVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.NewVacationRequestWaitingApproval, lang),
                NotificationType.AcceptingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.YourVacationIsAccepted, lang),
                NotificationType.RejectingVacationRequest => TranslationHelper.GetTranslation(AmgadKeys.RejectingVacationRequest, lang),
                NotificationType.NewTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.NewTaskRequestWaitingApproval, lang),
                NotificationType.AcceptingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.YourTaskRequestIsAccepted, lang),
                NotificationType.RejectingTaskRequest => TranslationHelper.GetTranslation(AmgadKeys.YourTaskRequestIsRejected, lang),
                NotificationType.NewPermissionRequent => TranslationHelper.GetTranslation(AmgadKeys.NewPermisionRequestWaitingApproval, lang),
                NotificationType.AcceptingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.YourPermissionRequestIsAccepted, lang),
                NotificationType.RejectingPermissionRequest => TranslationHelper.GetTranslation(AmgadKeys.YourPermissionRequestIsRejected, lang),
                NotificationType.NewJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.NewJustificationRequestWaitingApproval, lang),
                NotificationType.AcceptingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.YourJustificationRequestIsAccepted, lang),
                NotificationType.RejectingJustificationRequest => TranslationHelper.GetTranslation(AmgadKeys.YourJustificationRequestIsRejected, lang),
                NotificationType.NewAssignmentRequest => TranslationHelper.GetTranslation(AmgadKeys.NewAssignmentRequestWaitingApproval, lang),
                NotificationType.AcceptingAssignmentRequest => TranslationHelper.GetTranslation(AmgadKeys.YourAssignmentRequestIsAccepted, lang),
                NotificationType.RejectingAssignmentRequest => TranslationHelper.GetTranslation(AmgadKeys.YourAssignmentRequestIsRejected, lang),
                NotificationType.AddingInMission => TranslationHelper.GetTranslation(AmgadKeys.AddingInMission, lang),
                NotificationType.NewSummon => TranslationHelper.GetTranslation(AmgadKeys.NewSummonDescription, lang),
                NotificationType.NewOvertimeRequest => TranslationHelper.GetTranslation(LeillaKeys.NewOvertimeRequestDescription, lang),
                NotificationType.AcceptingOvertimeRequest => TranslationHelper.GetTranslation(LeillaKeys.YourOvertimeRequestIsAccepted, lang),
                NotificationType.RejectingOvertimeRequest => TranslationHelper.GetTranslation(LeillaKeys.YourOvertimeRequestIsRejected, lang),
                _ => TranslationHelper.GetTranslation(AmgadKeys.Unknown, lang),
            };
        }
    }
}
