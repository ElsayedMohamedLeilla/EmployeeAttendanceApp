using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessLogic.Requests;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Requests;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Requests;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Attendances;
using Dawem.Models.Dtos.Others;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Dtos.Requests.Tasks;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Requests;
using Dawem.Models.Response.Requests.Tasks;
using Dawem.RealTime.Helper;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Requests.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Requests
{
    public class RequestTaskBL : IRequestTaskBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRequestTaskBLValidation requestTaskBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly INotificationStoreBL notificationStoreBL;
        private readonly INotificationServiceByFireBaseAdmin notificationServiceByFireBaseAdmin;
        public RequestTaskBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IRequestTaskBLValidation _requestTaskBLValidation,
           INotificationStoreBL _notificationStoreBL,
            INotificationServiceByFireBaseAdmin _notificationServiceByFireBaseAdmin)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            requestTaskBLValidation = _requestTaskBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
            notificationStoreBL = _notificationStoreBL;
            notificationServiceByFireBaseAdmin = _notificationServiceByFireBaseAdmin;
        }
        public async Task<int> Create(CreateRequestTaskModelDTO model)
        {
            #region Model Validation

            var validator = new CreateRequestTaskModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestTaskBLValidation.CreateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.TaskRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Request Task

            #region Set Request Task Code

            var getRequestNextCode = await repositoryManager.RequestRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            var getRequestTaskNextCode = await repositoryManager.RequestRepository
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
            request.RequestTask.Code = getRequestTaskNextCode;
            request.Status = RequestStatus.Pending;
            request.IsActive = true;
            request.RequestTask.IsActive = true;

            repositoryManager.RequestRepository.Insert(request);
            await unitOfWork.SaveAsync();

            #endregion

            #region Save Notification In DB
            for (int i = 0; i < model.TaskEmployeeIds.Count; i++)
            {
                var getNotificationNextCode = await repositoryManager.NotificationStoreRepository
              .Get(e => e.CompanyId == requestInfo.CompanyId)
              .Select(e => e.Code)
              .DefaultIfEmpty()
              .MaxAsync() + 1;
                var notificationStore = new NotificationStore()
                {
                    Code = getNotificationNextCode,
                    EmployeeId = model.TaskEmployeeIds[i],
                    CompanyId = requestInfo.CompanyId,
                    AddUserId = requestInfo.UserId,
                    AddedDate = DateTime.UtcNow,
                    Status = NotificationStatus.Info,
                    NotificationType = NotificationType.NewTaskRequest,
                    ImageUrl = NotificationHelper.GetNotificationImage(NotificationStatus.Info, uploadBLC),
                    IsRead = false,
                    IsActive = true,
                    Priority = Priority.Medium,
                };
                repositoryManager.NotificationStoreRepository.Insert(notificationStore);
                await unitOfWork.SaveAsync();
            }
            #endregion
            #region Fire Notification & Email
            List<int> userIds = repositoryManager.UserRepository.Get(s => !s.IsDeleted && s.IsActive & model.TaskEmployeeIds.Contains(s.EmployeeId ?? 0)).Select(u => u.Id).ToList();
            if (userIds.Count > 0)
            {
                await notificationServiceByFireBaseAdmin.Send_Notification_Email(userIds, NotificationType.NewTaskRequest, NotificationStatus.Info);
            }
            #endregion



            #region Handle Response

            await unitOfWork.CommitAsync();
            return request.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateRequestTaskModelDTO model)
        {
            #region Model Validation

            var validator = new UpdateRequestTaskModelDTOValidator();
            var validatorResult = validator.Validate(model);
            if (!validatorResult.IsValid)
            {
                var error = validatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            var employeeId = await requestTaskBLValidation.UpdateValidation(model);

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
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.TaskRequests)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadRequestAttachements); ;
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Request Task

            var getRequest = await repositoryManager.RequestRepository
                 .GetEntityByConditionWithTrackingAsync(request => !request.IsDeleted
                 && request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            var getRequestTask = await repositoryManager.RequestTaskRepository
                 .GetEntityByConditionWithTrackingAsync(requestTask => !requestTask.Request.IsDeleted
                 && requestTask.Request.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            getRequest.EmployeeId = model.EmployeeId ?? 0;
            getRequest.ForEmployee = model.ForEmployee;
            getRequest.Notes = model.Notes;
            getRequest.IsNecessary = model.IsNecessary;
            getRequest.Date = model.DateFrom;
            getRequest.ModifiedDate = DateTime.Now;
            getRequest.ModifyUserId = requestInfo.UserId;


            getRequestTask.ModifiedDate = DateTime.Now;
            getRequestTask.ModifyUserId = requestInfo.UserId;
            getRequestTask.DateTo = model.DateTo;

            await unitOfWork.SaveAsync();

            #region Update Task Employees 

            var existDbList = await repositoryManager.RequestTaskEmployeeRepository
                    .Get(e => e.RequestTaskId == getRequestTask.Id)
                    .ToListAsync();

            var existingEmployeeIds = existDbList.Select(e => e.EmployeeId).ToList();

            var addedTaskEmployees = model.TaskEmployeeIds
                .Where(employeeId => !existingEmployeeIds.Contains(employeeId))
                .Select(employeeId => new RequestTaskEmployee
                {
                    RequestTaskId = getRequestTask.Id,
                    EmployeeId = employeeId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();

            var removedTaskEmployeeIds = existDbList
                .Where(ge => !model.TaskEmployeeIds.Contains(ge.EmployeeId))
                .Select(ge => ge.EmployeeId)
                .ToList();

            var removedTaskEmployees = await repositoryManager.RequestTaskEmployeeRepository
                .Get(e => e.EmployeeId == model.Id && removedTaskEmployeeIds.Contains(e.EmployeeId))
                .ToListAsync();

            if (removedTaskEmployees.Count > 0)
                repositoryManager.RequestTaskEmployeeRepository.BulkDeleteIfExist(removedTaskEmployees);
            if (addedTaskEmployees.Count > 0)
                repositoryManager.RequestTaskEmployeeRepository.BulkInsert(addedTaskEmployees);

            await unitOfWork.SaveAsync();

            #endregion

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
        public async Task<GetRequestTasksResponse> Get(GetRequestTasksCriteria criteria)
        {
            var requestTaskRepository = repositoryManager.RequestTaskRepository;
            var query = requestTaskRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestTaskRepository.OrderBy(query, nameof(RequestTask.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestTasksList = await queryPaged.Select(requestTask => new GetRequestTasksResponseModel
            {
                Id = requestTask.Request.Id,
                Code = requestTask.Request.Code,
                Employee = new RequestEmployeeModel
                {
                    Code = requestTask.Request.Employee.Code,
                    Name = requestTask.Request.Employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(requestTask.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                },
                TaskTypeName = requestTask.TaskType.Name,
                DateFrom = requestTask.Request.Date,
                DateTo = requestTask.DateTo,
                Status = requestTask.Request.Status,
                StatusName = TranslationHelper.GetTranslation(requestTask.Request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestTasksResponse
            {
                TaskRequests = requestTasksList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<List<EmployeeGetRequestTasksResponseModel>> EmployeeGet(EmployeeGetRequestTasksCriteria criteria)
        {
            var resonse = new List<EmployeeGetRequestTasksResponseModel>();

            #region Business Validation

            var result = await requestTaskBLValidation.GetEmployeeTasksValidation(criteria);

            #endregion

            var getEmployeeId = requestInfo?.EmployeeId;

            var employeeTasks = await repositoryManager.RequestTaskRepository
                .Get(a => !a.Request.IsDeleted && a.TaskEmployees.Any(e => e.EmployeeId == getEmployeeId)
                && ((a.Request.Date.Month == criteria.Month
                && a.Request.Date.Year == criteria.Year) || (a.Request.RequestTask.DateTo.Month == criteria.Month
                && a.Request.RequestTask.DateTo.Year == criteria.Year)))
                .Select(requestTask => new
                {
                    requestTask.Id,
                    requestTask.Request.Code,
                    requestTask.Request.Date,
                    requestTask.DateTo,
                    requestTask.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestTask.Request.Status.ToString(), requestInfo.Lang),
                    TaskTypeName = requestTask.TaskType.Name,
                    Employees = requestTask.TaskEmployees.Select(e => new RequestEmployeeModel()
                    {
                        Code = e.Employee.Code,
                        Name = e.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(e.Employee.ProfileImageName, LeillaKeys.Employees)
                    }).ToList()
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
                var dayTasks = employeeTasks
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

                #endregion

                if (scheduleId != null && shiftId == null)
                {
                    weekVacationDays.Add(new DayAndWeekDayModel()
                    {
                        Day = date.Day,
                        WeekDay = (WeekDay)date.DayOfWeek
                    });
                }
                else
                {

                    var employeeGetRequestTasksResponseModel = new EmployeeGetRequestTasksResponseModel
                    {
                        DayTasks = new GetTaskDayModel
                        {
                            Day = date.Day,
                            WeekDay = (WeekDay)date.DayOfWeek,
                            WeekDayName = TranslationHelper.GetTranslation(((WeekDay)date.DayOfWeek).ToString(), requestInfo.Lang),
                            Tasks = dayTasks.Select(ds => new GetTaskModel
                            {
                                Id = ds.Id,
                                TaskTypeName = ds.TaskTypeName,
                                Code = ds.Code,
                                DateFrom = ds.Date,
                                DateTo = ds.DateTo,
                                Status = ds.Status,
                                StatusName = ds.StatusName,
                                Employees = ds.Employees
                            }).ToList()
                        }
                    };

                    resonse.Add(employeeGetRequestTasksResponseModel);
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
                    resonse.Add(new EmployeeGetRequestTasksResponseModel()
                    {
                        DayTasks = null,
                        Informations = TranslationHelper.GetTranslation(LeillaKeys.EndOfWeekVacations, requestInfo.Lang) + allVacationsText
                    });

                    weekVacationDays = new List<DayAndWeekDayModel>();
                }
            }

            return resonse;
        }
        public async Task<GetRequestTasksForDropDownResponse> GetForDropDown(GetRequestTasksCriteria criteria)
        {
            criteria.IsActive = true;
            var requestTaskRepository = repositoryManager.RequestTaskRepository;
            var query = requestTaskRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestTaskRepository.OrderBy(query, nameof(RequestTask.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestTasksList = await queryPaged.Select(e => new GetRequestTasksForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Request.Employee.Name
            }).ToListAsync();

            return new GetRequestTasksForDropDownResponse
            {
                TaskRequests = requestTasksList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestTaskInfoResponseModel> GetInfo(int requestId)
        {
            var requestTask = await repositoryManager.RequestTaskRepository.Get(e => e.Request.Id == requestId && !e.Request.IsDeleted)
                .Select(requestTask => new GetRequestTaskInfoResponseModel
                {
                    Code = requestTask.Request.Code,
                    Employee = new RequestEmployeeModel
                    {
                        Code = requestTask.Request.Employee.Code,
                        Name = requestTask.Request.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(requestTask.Request.Employee.ProfileImageName, LeillaKeys.Employees)
                    },
                    TaskTypeName = requestTask.TaskType.Name,
                    DateFrom = requestTask.Request.Date,
                    DateTo = requestTask.DateTo,
                    IsActive = requestTask.Request.IsActive,
                    IsNecessary = requestTask.Request.IsNecessary,
                    ForEmployee = requestTask.Request.ForEmployee,
                    TaskEmployees = requestTask.TaskEmployees.Select(te => te.Employee.Name).ToList(),
                    Attachments = requestTask.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.TaskRequests),
                    }).ToList(),
                    Status = requestTask.Request.Status,
                    StatusName = TranslationHelper.GetTranslation(requestTask.Request.Status.ToString(), requestInfo.Lang),
                    Notes = requestTask.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestTask;
        }
        public async Task<GetRequestTaskByIdResponseModel> GetById(int RequestTaskId)
        {
            var requestTask = await repositoryManager.RequestTaskRepository.Get(e => e.Request.Id == RequestTaskId && !e.IsDeleted)
                .Select(requestTask => new GetRequestTaskByIdResponseModel
                {
                    Id = requestTask.Request.Id,
                    Code = requestTask.Request.Code,
                    EmployeeId = requestTask.Request.EmployeeId,
                    TaskTypeId = requestTask.TaskTypeId,
                    DateFrom = requestTask.Request.Date,
                    DateTo = requestTask.DateTo,
                    IsActive = requestTask.Request.IsActive,
                    IsNecessary = requestTask.Request.IsNecessary,
                    ForEmployee = requestTask.Request.ForEmployee,
                    TaskEmployeeIds = requestTask.TaskEmployees.Select(te => te.EmployeeId).ToList(),
                    Attachments = requestTask.Request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.TaskRequests)
                    }).ToList(),
                    Notes = requestTask.Request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return requestTask;

        }
        public async Task<bool> Delete(int requestId)
        {
            var requestTask = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            requestTask.Delete();
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

            #region Save Notification In DB
            List<int> TaskEmployeeIds = new List<int>(); // get this list
            for (int i = 0; i < TaskEmployeeIds.Count; i++)
            {
                var getNotificationNextCode = await repositoryManager.NotificationStoreRepository
              .Get(e => e.CompanyId == requestInfo.CompanyId)
              .Select(e => e.Code)
              .DefaultIfEmpty()
              .MaxAsync() + 1;
                var notificationStore = new NotificationStore()
                {
                    Code = getNotificationNextCode,
                    EmployeeId = TaskEmployeeIds[i],
                    CompanyId = requestInfo.CompanyId,
                    AddUserId = requestInfo.UserId,
                    AddedDate = DateTime.UtcNow,
                    Status = NotificationStatus.Info,
                    NotificationType = NotificationType.NewTaskRequest,
                    ImageUrl = NotificationHelper.GetNotificationImage(NotificationStatus.Info, uploadBLC),
                    IsRead = false,
                    IsActive = true,
                    Priority = Priority.Medium,
                };
                repositoryManager.NotificationStoreRepository.Insert(notificationStore);
                await unitOfWork.SaveAsync();
            }
            #endregion
            #region Fire Notification & Email
            List<int> userIds = repositoryManager.UserRepository.Get(s => !s.IsDeleted && s.IsActive & TaskEmployeeIds.Contains(s.EmployeeId ?? 0)).Select(u => u.Id).ToList();
            if (userIds.Count > 0)
            {
                await notificationServiceByFireBaseAdmin.Send_Notification_Email(userIds, NotificationType.NewTaskRequest, NotificationStatus.Info);
            }
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
            return true;
        }
        public async Task<GetTasksInformationsResponseDTO> GetTasksInformations()
        {
            var requestTaskRepository = repositoryManager.RequestTaskRepository;
            var query = requestTaskRepository.Get(request => !request.Request.IsDeleted &&
            request.Request.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetTasksInformationsResponseDTO
            {
                TotalTasksCount = await query.CountAsync(),
                AcceptedCount = await query.Where(request => request.Request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
    }
}

