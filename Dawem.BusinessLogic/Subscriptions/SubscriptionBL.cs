using AutoMapper;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessLogic.Summons;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Summons;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Departments;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Summons
{
    public class SubscriptionBL : ISubscriptionBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISummonBLValidation summonBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IMailBL mailBL;
        private readonly IUploadBLC uploadBLC;
        private readonly INotificationServiceByFireBaseAdmin notificationServiceByFireBaseAdmin;



        public SubscriptionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper, IMailBL _mailBL,
           RequestInfo _requestHeaderContext,
           ISummonBLValidation _summonBLValidation, IUploadBLC _uploadBLC, INotificationServiceByFireBaseAdmin _notificationServiceByFireBaseAdmin)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            summonBLValidation = _summonBLValidation;
            mapper = _mapper;
            mailBL = _mailBL;
            uploadBLC = _uploadBLC;
            notificationServiceByFireBaseAdmin = _notificationServiceByFireBaseAdmin;
        }

        public async Task HandleSubscriptions()
        {
            try
            {
                #region ٍSend Expiration Email

                var getWillExpiredSubscriptions = await repositoryManager.SubscriptionRepository
                            .GetWithTracking(s => !s.IsDeleted &&
                            ((DateTime.Now.Date >= s.EndDate.Date && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpired)) ||
                            (EF.Functions.DateDiffDay(DateTime.Now.Date, s.EndDate.Date) == 1 && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter1Days)) ||
                            (EF.Functions.DateDiffDay(DateTime.Now.Date, s.EndDate.Date) == 3 && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter3Days)) ||
                            (EF.Functions.DateDiffDay(DateTime.Now.Date, s.EndDate.Date) == 7 && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter7Days))))
                            .ToListAsync();

                if (getWillExpiredSubscriptions != null && getWillExpiredSubscriptions.Count > 0)
                {
                    foreach (var subscription in getWillExpiredSubscriptions)
                    {
                        var getSubscriptionLogs = await repositoryManager.SubscriptionLogRepository
                            .Get(l => !l.IsDeleted && l.SubscriptionId == subscription.Id && l.EndDate == subscription.EndDate)
                            .ToListAsync();

                        var verifyEmail = new VerifyEmailModel
                        {
                            Email = subscription.FollowUpEmail,
                            Subject = "تنبيه لإشتراكك علي داوم"
                        };

                        #region Send Email

                        if (DateTime.Now.Date >= subscription.EndDate.Date &&
                            !getSubscriptionLogs.Any(l => l.LogType == SubscriptionLogType.SendEmailAboutExpired))
                        {
                            verifyEmail.Body = @"<meta charset='UTF-8'>
                                            <title>لقد انتهي إشتراكك علي داوم</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>مرحباً</h1>
                                            <h2>لقد انتهي إشتراكك علي داوم يرجي التواصل مع فريق دعم داوم للتجديد.</h2>
                                            <h1>تاريخ انتهاء الإشتراك:  " + subscription.EndDate.ToString("dd-MM-yyyy") + @"</h1>
                                            <p>فريق خدمة العملاء لشركة داوم يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:dawem.app.developers@gmail.com'>dawem.app.developers@gmail.com</a></p>
                                            <p> الهاتف: (+20)01234567
                                            </body>
                                            </html>";
                            await mailBL.SendEmail(verifyEmail);

                            repositoryManager.SubscriptionLogRepository.Insert(new()
                            {
                                SubscriptionId = subscription.Id,
                                EndDate = subscription.EndDate,
                                LogType = SubscriptionLogType.SendEmailAboutExpired,
                                LogTypeName = nameof(SubscriptionLogType.SendEmailAboutExpired)
                            });

                        }
                        if ((subscription.EndDate.Date - DateTime.Now.Date).TotalDays <= 1 &&
                            !getSubscriptionLogs.Any(l => l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter1Days))
                        {
                            verifyEmail.Body = @"<meta charset='UTF-8'>
                                            <title>لقد قارب إشتراكك علي داوم علي الانتهاء</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>مرحباً</h1>
                                            <h2>لقد قارب إشتراكك علي داوم علي الانتهاء.</h2>
                                            <h2>سوف ينتهي إشتراكك بعد يوم واحد.</h2>
                                            <h1>تاريخ انتهاء الإشتراك:  " + subscription.EndDate.ToString("dd-MM-yyyy") + @"</h1>
                                            <p>فريق خدمة العملاء لشركة داوم يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:dawem.app.developers@gmail.com'>dawem.app.developers@gmail.com</a></p>
                                            <p> الهاتف: (+20)01234567
                                            </body>
                                            </html>";
                            await mailBL.SendEmail(verifyEmail);

                            repositoryManager.SubscriptionLogRepository.Insert(new()
                            {
                                SubscriptionId = subscription.Id,
                                EndDate = subscription.EndDate,
                                LogType = SubscriptionLogType.SendEmailAboutExpirationAfter1Days,
                                LogTypeName = nameof(SubscriptionLogType.SendEmailAboutExpirationAfter1Days)
                            });

                        }
                        if ((subscription.EndDate.Date - DateTime.Now.Date).TotalDays <= 3 &&
                            !getSubscriptionLogs.Any(l => l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter3Days))
                        {
                            verifyEmail.Body = @"<meta charset='UTF-8'>
                                            <title>لقد قارب إشتراكك علي داوم علي الانتهاء</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>مرحباً</h1>
                                            <h2>لقد قارب إشتراكك علي داوم علي الانتهاء.</h2>
                                            <h2>سوف ينتهي إشتراكك بعد ثلاثة أيام.</h2>
                                            <h1>تاريخ انتهاء الإشتراك:  " + subscription.EndDate.ToString("dd-MM-yyyy") + @"</h1>
                                            <p>فريق خدمة العملاء لشركة داوم يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:dawem.app.developers@gmail.com'>dawem.app.developers@gmail.com</a></p>
                                            <p> الهاتف: (+20)01234567
                                            </body>
                                            </html>";
                            await mailBL.SendEmail(verifyEmail);

                            repositoryManager.SubscriptionLogRepository.Insert(new()
                            {
                                SubscriptionId = subscription.Id,
                                EndDate = subscription.EndDate,
                                LogType = SubscriptionLogType.SendEmailAboutExpirationAfter3Days,
                                LogTypeName = nameof(SubscriptionLogType.SendEmailAboutExpirationAfter3Days)
                            });

                        }
                        if ((subscription.EndDate.Date - DateTime.Now.Date).TotalDays <= 7 &&
                            !getSubscriptionLogs.Any(l => l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter7Days))
                        {
                            verifyEmail.Body = @"<meta charset='UTF-8'>
                                            <title>لقد قارب إشتراكك علي داوم علي الانتهاء</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>مرحباً</h1>
                                            <h2>لقد قارب إشتراكك علي داوم علي الانتهاء.</h2>
                                            <h2>سوف ينتهي إشتراكك بعد سبعة أيام.</h2>
                                            <h1>تاريخ انتهاء الإشتراك:  " + subscription.EndDate.ToString("dd-MM-yyyy") + @"</h1>
                                            <p>فريق خدمة العملاء لشركة داوم يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:dawem.app.developers@gmail.com'>dawem.app.developers@gmail.com</a></p>
                                            <p> الهاتف: (+20)01234567
                                            </body>
                                            </html>";
                            await mailBL.SendEmail(verifyEmail);

                            repositoryManager.SubscriptionLogRepository.Insert(new()
                            {
                                SubscriptionId = subscription.Id,
                                EndDate = subscription.EndDate,
                                LogType = SubscriptionLogType.SendEmailAboutExpirationAfter7Days,
                                LogTypeName = nameof(SubscriptionLogType.SendEmailAboutExpirationAfter7Days)
                            });

                        }

                        #endregion

                        await unitOfWork.SaveAsync();
                    }
                }

                #endregion

                #region Deactivate Expired

                var getExpiredSubscriptions = await repositoryManager.SubscriptionRepository
                            .GetWithTracking(s => !s.IsDeleted && DateTime.Now.Date >=
                            s.EndDate && s.Status != SubscriptionStatus.Deactivated)
                            .ToListAsync();

                if (getExpiredSubscriptions != null && getExpiredSubscriptions.Count > 0)
                {
                    var getGracePeriodPercentage = await repositoryManager.DawemSettingRepository
                        .Get(d => !d.IsDeleted && d.Type == DawemSettingType.PlansGracePeriodPercentage)
                        .Select(d => d.Integer)
                        .FirstOrDefaultAsync() ?? 0;

                    foreach (var subscription in getExpiredSubscriptions)
                    {
                        var extraDays = getGracePeriodPercentage * subscription.DurationInDays / 100;
                        var newEndDate = subscription.EndDate.AddDays(extraDays);

                        if (DateTime.Now.Date >= newEndDate)
                        {
                            subscription.Status = SubscriptionStatus.Deactivated;

                            repositoryManager.SubscriptionLogRepository.Insert(new()
                            {
                                SubscriptionId = subscription.Id,
                                EndDate = subscription.EndDate,
                                LogType = SubscriptionLogType.Deactivated,
                                LogTypeName = nameof(SubscriptionLogType.Deactivated)
                            });

                            #region Send Deactivate Email

                            var verifyEmail = new VerifyEmailModel
                            {
                                Email = subscription.FollowUpEmail,
                                Subject = "تم إيقاف إشتراكك علي داوم",
                                Body = @"<meta charset='UTF-8'>
                                            <title>لقد انتهي إشتراكك علي داوم</title>
                                            <style>
                                            body { direction: rtl; }
                                            </style>
                                            </head>
                                            <body>
                                            <h1>مرحباً</h1>
                                            <h2>تم إيقاف إشتراكك علي داوم يرجي التواصل مع فريق دعم داوم للتجديد و إعادة التفعيل.</h2>
                                            <h1>تاريخ انتهاء الإشتراك:  " + subscription.EndDate.ToString("dd-MM-yyyy") + @"</h1>
                                            <p>فريق خدمة العملاء لشركة داوم يتطلع لخدمتك.</p>
                                            <p>للتواصل معنا:</p>
                                            <p> البريد الإلكتروني: <a href='mailto:dawem.app.developers@gmail.com'>dawem.app.developers@gmail.com</a></p>
                                            <p> الهاتف: (+20)01234567
                                            </body>
                                            </html>"
                            };
                            await mailBL.SendEmail(verifyEmail);

                            #endregion
                        }
                    }
                    await unitOfWork.SaveAsync();
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

