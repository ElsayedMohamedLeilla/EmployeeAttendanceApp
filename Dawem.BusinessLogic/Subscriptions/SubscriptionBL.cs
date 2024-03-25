using AutoMapper;
using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.BusinessLogic.Subscriptions;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Dtos.Subscriptions;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Subscriptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Dawem.BusinessLogic.Subscriptions
{
    public class SubscriptionBL : ISubscriptionBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISubscriptionBLValidation subscriptionBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly IMailBL mailBL;

        public SubscriptionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IMailBL _mailBL,
           RequestInfo _requestHeaderContext,
           IUploadBLC _uploadBLC,

           ISubscriptionBLValidation _subscriptionBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            subscriptionBLValidation = _subscriptionBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
            mailBL = _mailBL;
        }
        public async Task<int> Create(CreateSubscriptionModel model)
        {
            #region Business Validation

            await subscriptionBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Subscription

            #region Set Subscription Code

            var getNextCode = await repositoryManager.SubscriptionRepository
                .Get()
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var subscription = mapper.Map<Subscription>(model);
            subscription.CompanyId = requestInfo.CompanyId;
            subscription.AddUserId = requestInfo.UserId;
            subscription.AddedApplicationType = requestInfo.ApplicationType;
            subscription.Code = getNextCode;
            repositoryManager.SubscriptionRepository.Insert(subscription);

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return subscription.Id;

            #endregion
        }
        public async Task<bool> Update(UpdateSubscriptionModel model)
        {
            #region Business Validation

            await subscriptionBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Subscription

            var getSubscription = await repositoryManager.SubscriptionRepository.
                GetEntityByConditionWithTrackingAsync(subscription => !subscription.IsDeleted
            && subscription.Id == model.Id);

            if (getSubscription != null)
            {
                getSubscription.PlanId = model.PlanId;
                getSubscription.CompanyId = model.CompanyId;
                getSubscription.DurationInDays = model.DurationInDays;
                getSubscription.StartDate = model.StartDate;
                getSubscription.EndDate = model.EndDate;
                getSubscription.Status = model.Status;
                getSubscription.RenewalCount = model.RenewalCount;
                getSubscription.FollowUpEmail = model.FollowUpEmail;
                getSubscription.ModifiedDate = DateTime.Now;
                getSubscription.ModifyUserId = requestInfo.UserId;
                getSubscription.Notes = model.Notes;

                await unitOfWork.SaveAsync();

                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorrySubscriptionNotFound);


        }
        public async Task<GetSubscriptionsResponse> Get(GetSubscriptionsCriteria criteria)
        {
            var subscriptionRepository = repositoryManager.SubscriptionRepository;
            var query = subscriptionRepository.GetAsQueryable(criteria);
            var isArabic = requestInfo.Lang == LeillaKeys.Ar;

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = subscriptionRepository.OrderBy(query, nameof(Subscription.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var subscriptionsList = await queryPaged.Select(subscription => new GetSubscriptionsResponseModel
            {
                Id = subscription.Id,
                Code = subscription.Code,
                PlanName = subscription.Plan.PlanNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                CompanyName = subscription.Company.Name,
                EndDate = subscription.EndDate,
                Status = subscription.Status,
                StatusName = TranslationHelper.GetTranslation(nameof(SubscriptionStatus) + LeillaKeys.Dash + subscription.Status.ToString(), requestInfo.Lang)
            }).ToListAsync();

            return new GetSubscriptionsResponse
            {
                Subscriptions = subscriptionsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetSubscriptionInfoResponseModel> GetInfo(int subscriptionId)
        {
            var isArabic = requestInfo.Lang == LeillaKeys.Ar;
            var subscription = await repositoryManager.SubscriptionRepository.Get(e => e.Id == subscriptionId && !e.IsDeleted)
                .Select(subscription => new GetSubscriptionInfoResponseModel
                {
                    Code = subscription.Code,
                    CompanyName = subscription.Company.Name,
                    PlanName = subscription.Plan.PlanNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    DurationInDays = subscription.DurationInDays,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    Status = subscription.Status,
                    StatusName = TranslationHelper.GetTranslation(nameof(SubscriptionStatus) + LeillaKeys.Dash + subscription.Status.ToString(), requestInfo.Lang),
                    FollowUpEmail = subscription.FollowUpEmail,
                    RenewalCount = subscription.RenewalCount,
                    Notes = subscription.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySubscriptionNotFound);

            return subscription;
        }
        public async Task<GetSubscriptionByIdResponseModel> GetById(int subscriptionId)
        {
            var subscription = await repositoryManager.SubscriptionRepository.Get(e => e.Id == subscriptionId && !e.IsDeleted)
                .Select(subscription => new GetSubscriptionByIdResponseModel
                {
                    Id = subscription.Id,
                    Code = subscription.Code,
                    CompanyId = subscription.CompanyId,
                    PlanId = subscription.PlanId,
                    DurationInDays = subscription.DurationInDays,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    Status = subscription.Status,
                    FollowUpEmail = subscription.FollowUpEmail,
                    RenewalCount = subscription.RenewalCount,
                    Notes = subscription.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySubscriptionNotFound);

            return subscription;

        }
        public async Task<bool> Delete(int subscriptiond)
        {
            var subscription = await repositoryManager.SubscriptionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == subscriptiond) ??
                throw new BusinessValidationException(LeillaKeys.SorrySubscriptionNotFound);
            subscription.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Enable(int subscriptionId)
        {
            var subscription = await repositoryManager.SubscriptionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == subscriptionId) ??
                throw new BusinessValidationException(LeillaKeys.SorrySubscriptionNotFound);
            subscription.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.SubscriptionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorrySubscriptionNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetSubscriptionsInformationsResponseDTO> GetSubscriptionsInformations()
        {
            var subscriptionRepository = repositoryManager.SubscriptionRepository;
            var query = subscriptionRepository.Get(subscription => subscription.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetSubscriptionsInformationsResponseDTO
            {
                TotalCount = await query.Where(subscription => !subscription.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(subscription => !subscription.IsDeleted && subscription.IsActive).CountAsync(),
                NotActiveCount = await query.Where(subscription => !subscription.IsDeleted && !subscription.IsActive).CountAsync(),
                DeletedCount = await query.Where(subscription => subscription.IsDeleted).CountAsync()
            };

            #endregion
        }
        public async Task HandleSubscriptions()
        {
            try
            {
                #region ٍSend Expiration Email

                var getWillExpiredSubscriptions = await repositoryManager.SubscriptionRepository
                            .GetWithTracking(s => !s.IsDeleted &&
                            (DateTime.Now.Date >= s.EndDate.Date && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpired) ||
                            EF.Functions.DateDiffDay(DateTime.Now.Date, s.EndDate.Date) == 1 && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter1Days) ||
                            EF.Functions.DateDiffDay(DateTime.Now.Date, s.EndDate.Date) == 3 && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter3Days) ||
                            EF.Functions.DateDiffDay(DateTime.Now.Date, s.EndDate.Date) == 7 && !s.SubscriptionLogs.Any(l => l.EndDate.Date == s.EndDate.Date && l.LogType == SubscriptionLogType.SendEmailAboutExpirationAfter7Days)))
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

