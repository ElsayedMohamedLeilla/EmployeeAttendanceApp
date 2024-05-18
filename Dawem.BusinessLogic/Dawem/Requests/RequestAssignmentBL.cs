using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Requests;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.RealTime.Firebase;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.Models.Requests;
using Dawem.Models.Requests.Assignments;
using Dawem.Models.Response.Dawem.Requests;
using Dawem.Models.Response.Dawem.Requests.Assignments;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Dawem.Requests.Assignments;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Requests
{
    public class RequestAssignmentBL : IRequestAssignmentBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestAssignmentBLValidation requestAssignmentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly INotificationHandleBL notificationHandleBL;

        public RequestAssignmentBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestAssignmentBLValidation _requestAssignmentBLValidation,
           INotificationHandleBL _notificationHandleBL)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestAssignmentBLValidation = _requestAssignmentBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
            notificationHandleBL = _notificationHandleBL;

        }
        public async Task<int> Create(CreateRequestAssignmentModelDTO model)
        {
            #region Model Validation

            var validator = new CreateRequestAssignmentModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestAssignmentBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Files

            List<string> fileNames = null;

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                fileNames = new List<string>();

                foreach (var attachment in model.Attachments)
                {
                    if (attachment != null && attachment.Length > 0)
                    {
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.AssignmentRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements);
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Request Assignment

            #region Set Request Assignment Code

            var getRequestNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            var getRequestAssignmentNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var request = mapper.Map<Request>(model);
            request.CompanyId = requestInfo.CompanyId;
            request.AddUserId = requestInfo.UserId;
            request.EmployeeId = employeeId ?? 0;
            request.Code = getRequestNextCode;
            request.RequestAssignment.Code = getRequestAssignmentNextCode;
            request.Status = requestInfo.ApplicationType == ApplicationType.Web ? RequestStatus.Accepted : RequestStatus.Pending;
            request.IsActive = true;
            request.RequestAssignment.IsActive = true;

            repositoryManager.RequestRepository.Insert(request);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Notifications

            var requestEmployee = await repositoryManager
                .EmployeeRepository.Get(r => r.Id == employeeId && r.DirectManagerId > 0)
                .Select(e => new
                {
                    DirectManagerId = e.DirectManagerId.Value
                }).FirstOrDefaultAsync();

            var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == requestEmployee.DirectManagerId).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.
                    Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList()
                }).ToListAsync();

            var employeeIds = new List<int>() { requestEmployee.DirectManagerId };

            var handleNotificationModel = new HandleNotificationModel
            {
                NotificationUsers = notificationUsers,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.NewAssignmentRequest,
                NotificationStatus = NotificationStatus.Info,
                Priority = NotificationPriority.Medium
            };

            await notificationHandleBL.HandleNotifications(handleNotificationModel);

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return request.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateRequestAssignmentModelDTO model)
        {
            #region Model Validation

            var validator = new UpdateRequestAssignmentModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestAssignmentBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Files

            var newFileNames = new List<string>();

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                newFileNames = new List<string>();

                foreach (var attachment in model.Attachments)
                {
                    if (attachment != null && attachment.Length > 0)
                    {
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.AssignmentRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements);
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Request Assignment

            var getRequest = await repositoryManager.RequestRepository
                 .GetEntityByConditionWithTrackingAsync(request => !request.IsDeleted
                 && request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            var getRequestAssignment = await repositoryManager.RequestAssignmentRepository
                 .GetEntityByConditionWithTrackingAsync(requestAssignment => !requestAssignment.Request.IsDeleted
                 && requestAssignment.Request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            getRequest.EmployeeId = employeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.Notes = model.Notes;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.DateFrom;
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;


            getRequestAssignment.AssignmentTypeId = model.AssignmentTypeId;
            getRequestAssignment.ModifiedDate = DateTime.Now;
            getRequestAssignment.ModifyUserId = requestInfo.UserId;
            getRequestAssignment.DateTo = model.DateTo;
            getRequestAssignment.Notes = model.Notes;

            await unitOfWork.SaveAsync();

            #region Update Attachements 

            var existAttachementsDbList = await repositoryManager.RequestAttachmentRepository
                    .Get(e => e.RequestId == getRequest.Id)
                    .ToListAsync();

            var existingFileNames = existAttachementsDbList.Select(e => e.FileName).ToList();

            var addedAttachements = newFileNames
                .Select(fileName => new RequestAttachment
                {
                    RequestId = getRequest.Id,
                    FileName = fileName,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();

            var removedFileNames = existAttachementsDbList
                .Where(ge => model.AttachmentsNames == null || !model.AttachmentsNames.Contains(ge.FileName))
                .Select(ge => ge.FileName)
                .ToList();

            var removedAttachments = await repositoryManager.RequestAttachmentRepository
                .Get(e => e.RequestId == getRequest.Id && removedFileNames.Contains(e.FileName))
                .ToListAsync();

            if (removedAttachments.Count > 0)
                repositoryManager.RequestAttachmentRepository.BulkDeleteIfExist(removedAttachments);
            if (addedAttachements.Count > 0)
                repositoryManager.RequestAttachmentRepository.BulkInsert(addedAttachements);

            await unitOfWork.SaveAsync();

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();

            return true;

            #endregion
        }
        public async Task<GetRequestAssignmentsResponse> Get(GetRequestAssignmentsCriteria criteria)
        {
            var requestAssignmentRepository = repositoryManager.RequestAssignmentRepository;
            var query = requestAssignmentRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestAssignmentRepository.OrderBy(query, nameof(RequestAssignment.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestAssignmentsList = await queryPaged.Select(requestAssignment => new GetRequestAssignmentsResponseModel
            {
                Id = requestAssignment.Request.Id,
                Code = requestAssignment.Request.Code,
                Employee = new RequestEmployeeModel
                {
                    EmployeeNumber = requestAssignment.Request.Employee.EmployeeNumber,
                    Name = requestAssignment.Request.Employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(requestAssignment.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                },
                AssignmentTypeName = requestAssignment.AssignmentType.Name,
                DateFrom = requestAssignment.Request.Date,
                DateTo = requestAssignment.DateTo,
                Status = requestAssignment.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestAssignment.Request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestAssignmentsResponse
            {
                AssignmentRequests = requestAssignmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<List<EmployeeGetRequestAssignmentsResponseModel>> EmployeeGet(EmployeeGetRequestAssignmentsCriteria criteria)
        {
            var resonse = new List<EmployeeGetRequestAssignmentsResponseModel>();

            #region Business Validation

            var result = await requestAssignmentBLValidation.GetEmployeeAssignmentsValidation(criteria);

            #endregion

            var getEmployeeId = requestInfo?.EmployeeId;

            var employeeAssignments = await repositoryManager.RequestAssignmentRepository
                .Get(a => !a.Request.IsDeleted && a.Request.EmployeeId == getEmployeeId
                && a.Request.Date.Month == criteria.Month
                && a.Request.Date.Year == criteria.Year)
                .Select(requestAssignment => new
                {
                    requestAssignment.Request.Id,
                    requestAssignment.Request.Code,
                    requestAssignment.Request.Date,
                    requestAssignment.DateTo,
                    requestAssignment.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestAssignment.Request.Status.ToString(), requestInfo.Lang),
                    AssignmentTypeName = requestAssignment.AssignmentType.Name,
                    Employees = new List<RequestEmployeeModel>
                    {
                        new RequestEmployeeModel()
                        {
                            EmployeeNumber = requestAssignment.Request.Employee.EmployeeNumber,
                            Name = requestAssignment.Request.Employee.Name,
                            ProfileImagePath = uploadBLC.GetFilePath(requestAssignment.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                        }
                    }
                }).ToListAsync();

            var allDatesInMonth = OthersHelper.AllDatesInMonth(criteria.Year, criteria.Month).ToList();
            var maxDate = allDatesInMonth[allDatesInMonth.Count - 1];

            var employeePlans = await repositoryManager.SchedulePlanRepository.Get(s => !s.IsDeleted && s.DateFrom.Date <= maxDate.Date &&
                (s.SchedulePlanEmployee != null && s.SchedulePlanEmployee.EmployeeId == getEmployeeId ||
                s.SchedulePlanGroup != null && s.SchedulePlanGroup.Group.GroupEmployees != null && s.SchedulePlanGroup.Group.GroupEmployees.Any(g => g.EmployeeId == getEmployeeId) ||
                s.SchedulePlanDepartment != null && s.SchedulePlanDepartment.Department.Employees != null && s.SchedulePlanDepartment.Department.Employees.Any(g => g.Id == getEmployeeId)))
                .Select(s => new SchedulePlan
                {
                    DateFrom = s.DateFrom,
                    ScheduleId = s.ScheduleId
                }).ToListAsync();

            var employeePlansIds = employeePlans.Select(e => e.ScheduleId).ToList();

            var shifts = employeePlans != null ? await repositoryManager
                         .ScheduleDayRepository.Get(s => !s.IsDeleted && employeePlansIds.Contains(s.ScheduleId))
                         .Select(s => new
                         {
                             s.WeekDay,
                             s.ScheduleId,
                             s.ShiftId
                         }).ToListAsync() : null;

            var weekVacationDays = new List<DayAndWeekDayModel>();

            foreach (var date in allDatesInMonth)
            {
                var dayAssignments = employeeAssignments
                         .Where(e => e.Date.Date == date.Date)
                         .ToList();

                #region Check For Vacation

                var scheduleId = employeePlans.Where(s => s.DateFrom.Date <= date.Date)
                    .OrderByDescending(c => c.DateFrom.Date)?.FirstOrDefault()?.ScheduleId;

                int? shiftId = null;
                if (scheduleId != null)
                {
                    shiftId = shifts.FirstOrDefault(s => s.ScheduleId == scheduleId && s.WeekDay == (WeekDay)date.DayOfWeek)
                         .ShiftId;
                }

                var isScheduleVacationDay = false;

                #endregion

                if (scheduleId != null && shiftId == null)
                {
                    isScheduleVacationDay = true;
                    weekVacationDays.Add(new DayAndWeekDayModel()
                    {
                        Day = date.Day,
                        WeekDay = (WeekDay)date.DayOfWeek
                    });
                }
                if (!isScheduleVacationDay || dayAssignments.Count > 0)
                {
                    var employeeGetRequestAssignmentsResponseModel = new EmployeeGetRequestAssignmentsResponseModel
                    {
                        DayAssignments = new GetAssignmentDayModel
                        {
                            Day = date.Day,
                            WeekDay = (WeekDay)date.DayOfWeek,
                            WeekDayName = TranslationHelper.GetTranslation(((WeekDay)date.DayOfWeek).ToString(), requestInfo.Lang),
                            Assignments = dayAssignments.Select(ds => new GetAssignmentModel
                            {
                                Id = ds.Id,
                                AssignmentTypeName = ds.AssignmentTypeName,
                                Code = ds.Code,
                                DateFrom = ds.Date,
                                DateTo = ds.DateTo,
                                Status = ds.Status,
                                StatusName = ds.StatusName,
                                Employees = ds.Employees,
                                Notes = isScheduleVacationDay ?
                                TranslationHelper.GetTranslation(LeillaKeys.WeekVacation, requestInfo.Lang) : null
                            }).ToList()
                        }
                    };

                    resonse.Add(employeeGetRequestAssignmentsResponseModel);
                }

                if (date.DayOfWeek == (DayOfWeek)WeekDay.Friday && weekVacationDays.Count > 0)
                {
                    var allVacationsText = LeillaKeys.EmptyString;

                    for (int i = 0; i < weekVacationDays.Count; i++)
                    {
                        var item = weekVacationDays[i];
                        allVacationsText += item.Day + LeillaKeys.Space + TranslationHelper.GetTranslation(item.WeekDay.ToString(), requestInfo.Lang) +
                            (weekVacationDays.Count - i > 1 ? LeillaKeys.SpaceThenDashThenSpace : null);
                    }
                    resonse.Add(new EmployeeGetRequestAssignmentsResponseModel()
                    {
                        DayAssignments = null,
                        Informations = TranslationHelper.GetTranslation(LeillaKeys.EndOfWeekVacations, requestInfo.Lang) + allVacationsText
                    });

                    weekVacationDays = new List<DayAndWeekDayModel>();
                }
            }

            return resonse;
        }
        public async Task<GetRequestAssignmentsForDropDownResponse> GetForDropDown(GetRequestAssignmentsCriteria criteria)
        {
            criteria.IsActive = true;
            var requestAssignmentRepository = repositoryManager.RequestAssignmentRepository;
            var query = requestAssignmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestAssignmentRepository.OrderBy(query, nameof(RequestAssignment.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestAssignmentsList = await queryPaged.Select(e => new GetRequestAssignmentsForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Request.Employee.Name
            }).ToListAsync();

            return new GetRequestAssignmentsForDropDownResponse
            {
                AssignmentRequests = requestAssignmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestAssignmentInfoResponseModel> GetInfo(int requestId)
        {
            var requestAssignment = await repositoryManager.RequestAssignmentRepository.Get(e => e.Request.Id == requestId && !e.Request.IsDeleted)
                .Select(requestAssignment => new GetRequestAssignmentInfoResponseModel
                {
                    Code = requestAssignment.Request.Code,
                    Employee = new RequestEmployeeModel
                    {
                        EmployeeNumber = requestAssignment.Request.Employee.EmployeeNumber,
                        Name = requestAssignment.Request.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(requestAssignment.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                    },
                    AssignmentTypeName = requestAssignment.AssignmentType.Name,
                    DateFrom = requestAssignment.Request.Date,
                    DateTo = requestAssignment.DateTo,
                    IsActive = requestAssignment.Request.IsActive,
                    IsNecessary = requestAssignment.Request.IsNecessary,
                    ForEmployee = requestAssignment.Request.ForEmployee,
                    Attachments = requestAssignment.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.AssignmentRequests),
                    }).ToList(),
                    Status = requestAssignment.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestAssignment.Request.Status.ToString(), requestInfo.Lang),
                    Notes = requestAssignment.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestAssignment;
        }
        public async Task<GetRequestAssignmentByIdResponseModel> GetById(int RequestAssignmentId)
        {
            var requestAssignment = await repositoryManager.RequestAssignmentRepository.Get(e => e.Request.Id == RequestAssignmentId && !e.IsDeleted)
                .Select(requestAssignment => new GetRequestAssignmentByIdResponseModel
                {
                    Id = requestAssignment.Request.Id,
                    Code = requestAssignment.Request.Code,
                    EmployeeId = requestAssignment.Request.EmployeeId,
                    AssignmentTypeId = requestAssignment.AssignmentTypeId,
                    DateFrom = requestAssignment.Request.Date,
                    DateTo = requestAssignment.DateTo,
                    Status = requestAssignment.Request.Status,
                    IsActive = requestAssignment.Request.IsActive,
                    IsNecessary = requestAssignment.Request.IsNecessary,
                    ForEmployee = requestAssignment.Request.ForEmployee,
                    Attachments = requestAssignment.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.AssignmentRequests)
                    }).ToList(),
                    Notes = requestAssignment.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestAssignment;

        }
        public async Task<bool> Delete(int requestId)
        {
            var requestAssignment = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            requestAssignment.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Accept(int requestId)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
               throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            if (request.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyAccepted);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyRejected);
            }

            request.Status = RequestStatus.Accepted;
            request.DecisionUserId = requestInfo.UserId;
            request.DecisionDate = DateTime.UtcNow;

            await unitOfWork.SaveAsync();

            #region Handle Notifications

            var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == request.EmployeeId).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.Any() ? u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList() : null
                }).ToListAsync();

            var employeeIds = new List<int>() { request.EmployeeId };

            var handleNotificationModel = new HandleNotificationModel
            {
                NotificationUsers = notificationUsers,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.AcceptingAssignmentRequest,
                NotificationStatus = NotificationStatus.Info,
                Priority = NotificationPriority.Medium
            };

            await notificationHandleBL.HandleNotifications(handleNotificationModel);

            #endregion

            return true;
        }
        public async Task<bool> Reject(RejectModelDTO rejectModelDTO)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == rejectModelDTO.Id) ??
               throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            if (request.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyAccepted);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyRejected);
            }

            request.Status = RequestStatus.Rejected;
            request.DecisionUserId = requestInfo.UserId;
            request.DecisionDate = DateTime.UtcNow;
            request.RejectReason = rejectModelDTO.RejectReason;

            await unitOfWork.SaveAsync();

            #region Handle Notifications

            var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive &
                s.EmployeeId == request.EmployeeId).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.Any() ? u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList() : null
                }).ToListAsync();

            var employeeIds = new List<int>() { request.EmployeeId };

            var handleNotificationModel = new HandleNotificationModel
            {
                NotificationUsers = notificationUsers,
                EmployeeIds = employeeIds,
                NotificationType = NotificationType.RejectingAssignmentRequest,
                NotificationStatus = NotificationStatus.Info,
                Priority = NotificationPriority.Medium
            };

            await notificationHandleBL.HandleNotifications(handleNotificationModel);

            #endregion

            return true;
        }
        public async Task<GetAssignmentsInformationsResponseDTO> GetAssignmentsInformations()
        {
            var requestAssignmentRepository = repositoryManager.RequestAssignmentRepository;
            var query = requestAssignmentRepository.Get(requestAssignment => requestAssignment.Request.Type == RequestType.Assignment
            && !requestAssignment.Request.IsDeleted &&
            requestAssignment.Request.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetAssignmentsInformationsResponseDTO
            {
                TotalAssignmentsCount = await query.CountAsync(),
                AcceptedCount = await query.Where(request => request.Request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
    }
}

