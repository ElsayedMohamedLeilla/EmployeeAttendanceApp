using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Summons
{
    public class AlertBL : IAlertBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly INotificationHandleBL notificationHandleBL;

        public AlertBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            INotificationHandleBL _notificationHandleBL,
           RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            notificationHandleBL = _notificationHandleBL;
        }
        public async Task HandleSystemAlerts()
        {
            try
            {
                #region Helpers

                var utcDateTime = DateTime.UtcNow;
                var utcDate = DateTime.UtcNow.Date;
                var utcTime = DateTime.UtcNow.TimeOfDay;
                var utcDayOfWeek = DateTime.UtcNow.DayOfWeek;

                var getActiveLanguages = await repositoryManager.LanguageRepository.Get(l => !l.IsDeleted && l.IsActive).
                            Select(l => new ActiveLanguageModel
                            {
                                Id = l.Id,
                                ISO2 = l.ISO2
                            }).ToListAsync();

                #endregion

                #region Handle Do Not Forget Summons

                var getDoNotForgetSummonEmployeesList = await repositoryManager
                           .EmployeeRepository.Get(employee => !employee.IsDeleted &&
                           employee.Users.Any() &&
                           employee.SummonLogs.Any(sl => !sl.IsDeleted && !sl.DoneSummon &&
                           EF.Functions.DateDiffMinute(sl.Summon.StartDateAndTimeUTC, utcDateTime) >= 1 &&
                           utcDateTime < sl.Summon.EndDateAndTimeUTC &&
                           !employee.Company.Notifications.Any(en => !en.IsDeleted &&
                           en.NotificationEmployees.Any(ne => ne.EmployeeId == employee.Id) &&
                           en.NotificationType == NotificationType.DoNotForgetSummon &&
                           en.HelperNumber == sl.SummonId))).
                           Select(employee => new
                           {
                               employee.CompanyId,
                               EmployeeId = employee.Id,
                               UsersIds = employee.Users != null ? employee.Users.Where(u => !u.IsDeleted).Select(u => u.Id).ToList() : null,
                               SummonDate = employee.SummonLogs.FirstOrDefault(sl => !sl.IsDeleted && !sl.DoneSummon &&
                                EF.Functions.DateDiffMinute(sl.Summon.StartDateAndTimeUTC, utcDateTime) >= 1 &&
                                 utcDateTime < sl.Summon.EndDateAndTimeUTC &&
                                !employee.Company.Notifications.Any(en => !en.IsDeleted &&
                                en.NotificationType == NotificationType.DoNotForgetSummon &&
                                en.HelperNumber == sl.SummonId)).Summon.LocalDateAndTime,
                               employee.SummonLogs.FirstOrDefault(sl => !sl.IsDeleted && !sl.DoneSummon &&
                                EF.Functions.DateDiffMinute(sl.Summon.StartDateAndTimeUTC, utcDateTime) >= 1 &&
                                 utcDateTime < sl.Summon.EndDateAndTimeUTC &&
                                !employee.Company.Notifications.Any(en => !en.IsDeleted &&
                                en.NotificationType == NotificationType.DoNotForgetSummon &&
                                en.HelperNumber == sl.SummonId)).SummonId
                           }).ToListAsync();

                if (getDoNotForgetSummonEmployeesList != null && getDoNotForgetSummonEmployeesList.Count > 0)
                {
                    #region Handle Notifications

                    requestInfo.Lang = LeillaKeys.Ar;

                    var doNotForgetSummonEmployeesGroupedByCompany = getDoNotForgetSummonEmployeesList.
                        GroupBy(m => m.CompanyId).
                        ToList();

                    foreach (var doNotForgetSummonEmployeesCompanyGroup in doNotForgetSummonEmployeesGroupedByCompany)
                    {
                        var companyId = doNotForgetSummonEmployeesCompanyGroup.First().CompanyId;

                        var doNotForgetSummonEmployeesGroupedBySummon = doNotForgetSummonEmployeesCompanyGroup.
                            GroupBy(m => m.SummonId).
                            ToList();

                        foreach (var doNotForgetSummonGroup in doNotForgetSummonEmployeesGroupedBySummon)
                        {
                            var employeeIds = doNotForgetSummonGroup.Select(m => m.EmployeeId).ToList();
                            var userIds = doNotForgetSummonGroup.SelectMany(m => m.UsersIds).Distinct().ToList();
                            var summonDate = doNotForgetSummonGroup.First().SummonDate;
                            var summonId = doNotForgetSummonGroup.First().SummonId;


                            #region Handle Summon Description

                            var notificationDescriptions = new List<NotificationDescriptionModel>();

                            foreach (var language in getActiveLanguages)
                            {
                                notificationDescriptions.Add(new NotificationDescriptionModel
                                {
                                    LanguageIso2 = language.ISO2,
                                    Description = TranslationHelper.GetTranslation(LeillaKeys.DoNotForgetToFingerprintForTheSummonAssignedForYou, language.ISO2) +
                                        LeillaKeys.Space +
                                        TranslationHelper.GetTranslation(LeillaKeys.SummonDate, language.ISO2) +
                                        LeillaKeys.ColonsThenSpace +
                                        summonDate.ToString("dd-MM-yyyy") +
                                        LeillaKeys.Space +
                                        TranslationHelper.GetTranslation(LeillaKeys.TheTime, language.ISO2) +
                                        LeillaKeys.ColonsThenSpace +
                                        summonDate.ToString("hh:mm") + LeillaKeys.Space +
                                        DateHelper.TranslateAmAndPm(summonDate.ToString("tt"), language.ISO2)
                                });
                            }


                            #endregion

                            var handleNotificationModel = new HandleNotificationModel
                            {
                                CompanyId = companyId,
                                UserIds = userIds,
                                EmployeeIds = employeeIds,
                                NotificationType = NotificationType.DoNotForgetSummon,
                                NotificationStatus = NotificationStatus.Info,
                                Priority = NotificationPriority.Medium,
                                NotificationDescriptions = notificationDescriptions,
                                ActiveLanguages = getActiveLanguages,
                                HelperNumber = summonId
                            };

                            await notificationHandleBL.HandleNotifications(handleNotificationModel);
                        }
                    }

                    #endregion
                }

                #endregion

                #region Handle Do Not Forget To Sign-In

                var getDoNotForgetSignInEmployeesList = await repositoryManager
                           .EmployeeRepository.Get(employee => !employee.IsDeleted &&
                            employee.Users.Any() && employee.ScheduleId != null &&
                            !employee.EmployeeAttendances.Any(ea => !ea.IsDeleted && ea.LocalDate.Date == utcDate &&
                            ea.EmployeeAttendanceChecks.Any(eac => !eac.IsDeleted && eac.FingerPrintType == FingerPrintType.CheckIn)) &&
                            employee.Schedule.ScheduleDays.Any(sd => !sd.IsDeleted && sd.ShiftId != null &&
                            sd.WeekDay == (WeekDay)utcDateTime.DayOfWeek && utcTime < sd.Shift.CheckInTime &&
                            EF.Functions.DateDiffMinute((DateTime)(object)utcTime, (DateTime)(object)sd.Shift.CheckInTime) <= 15) &&
                            !employee.Company.Notifications.Any(en => !en.IsDeleted &&
                                en.NotificationEmployees.Any(ne => ne.EmployeeId == employee.Id) &&
                                en.NotificationType == NotificationType.DoNotForgetSignIn &&
                                en.HelperNumber == (int)utcDayOfWeek && en.HelperDate != null &&
                                en.HelperDate.Value.Date == utcDate)).
                           Select(employee => new
                           {
                               employee.CompanyId,
                               EmployeeId = employee.Id,
                               UsersIds = employee.Users != null ? employee.Users.Where(u => !u.IsDeleted).Select(u => u.Id).ToList() : null,
                               ShiftCheckInTime = employee.Schedule.ScheduleDays.FirstOrDefault(sd => !sd.IsDeleted && sd.ShiftId != null &&
                                    sd.WeekDay == (WeekDay)utcDateTime.DayOfWeek).Shift.CheckInTime
                           }).ToListAsync();

                if (getDoNotForgetSignInEmployeesList != null && getDoNotForgetSignInEmployeesList.Count > 0)
                {
                    #region Handle Notifications

                    requestInfo.Lang = LeillaKeys.Ar;

                    var doNotForgetSignInEmployeesGroupedBycompany = getDoNotForgetSignInEmployeesList.
                        GroupBy(m => m.CompanyId).
                        ToList();

                    foreach (var doNotForgetSignInEmployeesCompanyGroup in doNotForgetSignInEmployeesGroupedBycompany)
                    {
                        var companyId = doNotForgetSignInEmployeesCompanyGroup.First().CompanyId;

                        var doNotForgetSignInEmployeesGroupedByShiftCheckInTime = doNotForgetSignInEmployeesCompanyGroup.
                                GroupBy(m => m.ShiftCheckInTime).
                                ToList();
                        foreach (var doNotForgetSignInEmployeesGroup in doNotForgetSignInEmployeesGroupedByShiftCheckInTime)
                        {
                            var employeeIds = doNotForgetSignInEmployeesGroup.Select(m => m.EmployeeId).ToList();
                            var userIds = doNotForgetSignInEmployeesGroup.SelectMany(m => m.UsersIds).Distinct().ToList();
                            var shiftCheckInTime = new DateTime(doNotForgetSignInEmployeesGroup.First().ShiftCheckInTime.Ticks);
                           
                            #region Handle Summon Description

                            var notificationDescriptions = new List<NotificationDescriptionModel>();

                            foreach (var language in getActiveLanguages)
                            {
                                notificationDescriptions.Add(new NotificationDescriptionModel
                                {
                                    LanguageIso2 = language.ISO2,
                                    Description = TranslationHelper.GetTranslation(LeillaKeys.DoNotForgetToFingerprintForYourSignIn, language.ISO2) +
                                        LeillaKeys.Space +
                                        TranslationHelper.GetTranslation(LeillaKeys.CheckInTime, language.ISO2) +
                                        LeillaKeys.ColonsThenSpace +
                                        shiftCheckInTime.ToString("hh:mm") + LeillaKeys.Space +
                                        DateHelper.TranslateAmAndPm(shiftCheckInTime.ToString("tt"), language.ISO2)
                                });
                            }

                            #endregion

                            var handleNotificationModel = new HandleNotificationModel
                            {
                                CompanyId = companyId,
                                UserIds = userIds,
                                EmployeeIds = employeeIds,
                                NotificationType = NotificationType.DoNotForgetSignIn,
                                NotificationStatus = NotificationStatus.Info,
                                Priority = NotificationPriority.Medium,
                                NotificationDescriptions = notificationDescriptions,
                                ActiveLanguages = getActiveLanguages,
                                HelperNumber = (int)utcDateTime.DayOfWeek,
                                HelperDate = utcDateTime
                            };

                            await notificationHandleBL.HandleNotifications(handleNotificationModel);
                        }

                    }

                    #endregion
                }

                #endregion
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }
    }
}

