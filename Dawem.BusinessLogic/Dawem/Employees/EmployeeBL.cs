using AutoMapper;
using ClosedXML.Excel;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogic.Dawem.Employees;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Schedules;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Excel;
using Dawem.Models.Dtos.Dawem.Excel.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Employees.Employees;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.RealTime.Firebase;
using Dawem.Models.Response.Dawem.Employees.Employees;
using Dawem.Translations;
using Dawem.Validation.BusinessValidation.Dawem.ExcelValidations;
using Dawem.Validation.FluentValidation.Dawem.Employees.Employees;
using Microsoft.EntityFrameworkCore;



namespace Dawem.BusinessLogic.Dawem.Employees
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IEmployeeBLValidation employeeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        private readonly IMailBL mailBL;
        private readonly INotificationHandleBL notificationHandleBL;
        public EmployeeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper, INotificationHandleBL _notificationHandleBL,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IEmployeeBLValidation _employeeBLValidation, IMailBL _mailBL)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            employeeBLValidation = _employeeBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
            notificationHandleBL = _notificationHandleBL;
            mailBL = _mailBL;
        }
        public async Task<int> Create(CreateEmployeeModel model)
        {
            #region Model Validation

            #region Assign Delegatos In DepartmentZones Object

            if (model.ZoneIds != null && model.ZoneIds.Count > 0)
                model.MapEmployeeZones();

            #endregion

            var createEmployeeModel = new CreateEmployeeModelValidator();
            var createEmployeeModelResult = createEmployeeModel.Validate(model);
            if (!createEmployeeModelResult.IsValid)
            {
                var error = createEmployeeModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await employeeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Employees)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
                imageName = result.FileName;
            }

            #endregion

            #region Insert Employee


            #region Set Employee code

            var getNextCode = await repositoryManager.EmployeeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var employee = mapper.Map<Employee>(model);
            employee.CompanyId = requestInfo.CompanyId;
            employee.AddUserId = requestInfo.UserId;
            employee.AddedApplicationType = requestInfo.ApplicationType;
            employee.ProfileImageName = imageName;
            employee.Code = getNextCode;
            repositoryManager.EmployeeRepository.Insert(employee);
            await unitOfWork.SaveAsync();

            #region Send Email RegistrationInfo
            if (employee.IsActive)
            {
                #region get CompanyVerficationCode
                var CompanyVerificationCode = repositoryManager.CompanyRepository.Get(c => c.Id == requestInfo.CompanyId).Select(ss => ss.IdentityCode).FirstOrDefault();
                #endregion
                var verifyEmail = new VerifyEmailModel
                {
                    Email = employee.Email,
                    Subject = TranslationHelper.GetTranslation(AmgadKeys.RegistrationInformation, requestInfo?.Lang),
                    Body = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                                <title>{TranslationHelper.GetTranslation(AmgadKeys.RegistrationInformation, requestInfo?.Lang)} </title>
                            </head>
                            <body>
                                <p>
                                    {TranslationHelper.GetTranslation(AmgadKeys.PleaseUseThisInformationToCompleateYourSignUpOnMobileDontMakeAnyOneSeeThisInformation, requestInfo?.Lang)}
                                </p>
                                <p>
                                    <strong>  {TranslationHelper.GetTranslation(AmgadKeys.CompanyVerificationCode, requestInfo?.Lang) + " : " + LeillaKeys.Space + CompanyVerificationCode} </strong> 
                                </p>
                                <p>
                                    <strong>  {TranslationHelper.GetTranslation(AmgadKeys.EmploymentNumber, requestInfo?.Lang) + " : " + LeillaKeys.Space + employee.EmployeeNumber} </strong> 
                                </p>
                            </body>
                            </html>
                    "
                };

                await mailBL.SendEmail(verifyEmail);
            }
            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return employee.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateEmployeeModel model)
        {
            #region Model Validation

            var updateEmployeeModelValidator = new UpdateEmployeeModelValidator();
            var updateEmployeeModelValidatorResult = updateEmployeeModelValidator.Validate(model);
            if (!updateEmployeeModelValidatorResult.IsValid)
            {
                var error = updateEmployeeModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await employeeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region assign Delegatos In DepartmentZones Object
            model.MapEmployeeZones();
            #endregion

            #region Upload Profile Image

            string imageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Employees)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
                imageName = result.FileName;
            }

            #endregion

            #region Update Employee

            var getEmployee = await repositoryManager.EmployeeRepository
                .GetEntityByConditionWithTrackingAsync(employee => !employee.IsDeleted
            && employee.Id == model.Id);

            var oldScheduleId = getEmployee.ScheduleId;
            var newScheduleId = model.ScheduleId;

            getEmployee.Name = model.Name;
            getEmployee.DepartmentId = model.DepartmentId;
            getEmployee.IsActive = model.IsActive;
            getEmployee.JoiningDate = model.JoiningDate;
            getEmployee.ModifiedDate = DateTime.Now;
            getEmployee.ModifyUserId = requestInfo.UserId;
            getEmployee.AttendanceType = model.AttendanceType;
            getEmployee.EmployeeType = model.EmployeeType;
            getEmployee.JobTitleId = model.JobTitleId;
            getEmployee.DirectManagerId = model.DirectManagerId;
            getEmployee.MobileCountryId = model.MobileCountryId;
            getEmployee.ScheduleId = model.ScheduleId;
            getEmployee.EmployeeNumber = model.EmployeeNumber;
            getEmployee.Email = model.Email;
            getEmployee.AllowChangeFingerprintMobileCode = model.AllowChangeFingerprintMobileCode;
            getEmployee.AllowFingerprintOutsideAllowedZones = model.AllowFingerprintOutsideAllowedZones;
            getEmployee.MobileNumber = model.MobileNumber;
            getEmployee.Address = model.Address;
            getEmployee.ProfileImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.ProfileImageName)
                ? getEmployee.ProfileImageName : null;
            getEmployee.ModifiedApplicationType = requestInfo.ApplicationType;

            await unitOfWork.SaveAsync();

            #region Update ZoneEmployee

            var existDbList = repositoryManager.ZoneEmployeeRepository
                    .GetByCondition(e => e.EmployeeId == getEmployee.Id)
                    .ToList();

            var existingZoneIds = existDbList.Select(e => e.ZoneId).ToList();

            var addedEmployeeZones = model.Zones != null ? model.Zones
                .Where(ge => !existingZoneIds.Contains(ge.ZoneId))
                .Select(ge => new ZoneEmployee
                {
                    EmployeeId = model.Id,
                    ZoneId = ge.ZoneId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<ZoneEmployee>();

            var ZonesToRemove = existDbList
                .Where(ge => model.ZoneIds == null || !model.ZoneIds.Contains(ge.ZoneId))
                .Select(ge => ge.ZoneId)
                .ToList();

            var removedEmployeeZones = repositoryManager.ZoneEmployeeRepository
                .GetByCondition(e => e.EmployeeId == model.Id && ZonesToRemove.Contains(e.ZoneId))
                .ToList();

            if (removedEmployeeZones.Count > 0)
                repositoryManager.ZoneEmployeeRepository.BulkDeleteIfExist(removedEmployeeZones);
            if (addedEmployeeZones.Count > 0)
                repositoryManager.ZoneEmployeeRepository.BulkInsert(addedEmployeeZones);

            #endregion

            //await unitOfWork.SaveAsync();

            #region Enable Related user
            var user = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.EmployeeId == model.Id);
          
            #endregion
            if(user != null)
            {
                if (model.IsActive == true)
                {
                    user.IsActive = true;
                }
                else
                {
                    user.IsActive = true;
                }
            }
            
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Notifications

            if (oldScheduleId != newScheduleId)
            {
                var employeeIds = new List<int> { getEmployee.Id };

                var scheduleNames = await repositoryManager.ScheduleRepository.
                    Get(s => !s.IsDeleted && (s.Id == oldScheduleId || s.Id == newScheduleId)).
                    Select(s =>
                    new
                    {
                        s.Id,
                        s.Name
                    }).ToListAsync();

                var oldScheduleName = scheduleNames?.FirstOrDefault(s => s.Id == oldScheduleId)?.Name;
                var newScheduleName = scheduleNames?.FirstOrDefault(s => s.Id == newScheduleId)?.Name;

                var notificationUsers = await repositoryManager.UserRepository.
                Get(s => !s.IsDeleted && s.IsActive & s.EmployeeId > 0 &
                employeeIds.Contains(s.EmployeeId.Value)).
                Select(u => new NotificationUserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserTokens = u.NotificationUsers.
                    Where(nu => !nu.IsDeleted && nu.NotificationUserFCMTokens.
                    Any(f => !f.IsDeleted)).
                    SelectMany(nu => nu.NotificationUserFCMTokens.Where(f => !f.IsDeleted).
                    Select(f => new NotificationUserTokenModel
                    {
                        ApplicationType = f.DeviceType,
                        Token = f.FCMToken
                    })).ToList()
                }).ToListAsync();

                #region Handle Notification Description

                var notificationDescriptions = new List<NotificationDescriptionModel>();

                var getActiveLanguages = await repositoryManager.LanguageRepository.Get(l => !l.IsDeleted && l.IsActive).
                       Select(l => new ActiveLanguageModel
                       {
                           Id = l.Id,
                           ISO2 = l.ISO2
                       }).ToListAsync();

                foreach (var language in getActiveLanguages)
                {
                    notificationDescriptions.Add(new NotificationDescriptionModel
                    {
                        LanguageIso2 = language.ISO2,
                        Description = TranslationHelper.GetTranslation(LeillaKeys.YourScheduleHaveBeenChangedToNewSchedule, language.ISO2) +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.OldSchedule, language.ISO2) +
                            LeillaKeys.ColonsThenSpace +
                            (oldScheduleName ?? TranslationHelper.GetTranslation(LeillaKeys.NotExist, language.ISO2)) +
                            LeillaKeys.Space +
                            TranslationHelper.GetTranslation(LeillaKeys.NewSchedule, language.ISO2) +
                            LeillaKeys.ColonsThenSpace +
                            (newScheduleName ?? TranslationHelper.GetTranslation(LeillaKeys.NotExist, language.ISO2))
                    });
                }

                #endregion

                var handleNotificationModel = new HandleNotificationModel
                {
                    NotificationUsers = notificationUsers,
                    EmployeeIds = employeeIds,
                    NotificationType = NotificationType.NewChangeInSchedule,
                    NotificationStatus = NotificationStatus.Info,
                    Priority = NotificationPriority.Medium,
                    NotificationDescriptions = notificationDescriptions,
                    ActiveLanguages = getActiveLanguages
                };

                await notificationHandleBL.HandleNotifications(handleNotificationModel);
            }

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetEmployeesResponse> Get(GetEmployeesCriteria criteria)
        {
            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(e => new GetEmployeesResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DapartmentName = e.Department.Name,
                IsActive = e.IsActive,
                JoiningDate = e.JoiningDate,
                EmployeeNumber = e.EmployeeNumber,
                ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees)
            }).ToListAsync();

            return new GetEmployeesResponse
            {
                Employees = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetEmployeesForDropDownResponse> GetForDropDown(GetEmployeesCriteria criteria)
        {
            criteria.IsActive = true;
            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(e => new GetEmployeesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetEmployeesForDropDownResponse
            {
                Employees = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }

        public async Task<GetEmployeeInfoResponseModel> GetInfo(int employeeId)
        {
            var isArabic = requestInfo.Lang == LeillaKeys.Ar;

            var employee = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(e => new GetEmployeeInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    DapartmentName = e.Department.Name,
                    DirectManagerName = e.DirectManager.Name,
                    Email = e.Email,
                    MobileCountryCode = LeillaKeys.PlusSign + LeillaKeys.Space + e.MobileCountry.Dial,
                    MobileCountryName = isArabic ? e.MobileCountry.NameAr : e.MobileCountry.NameEn,
                    MobileCountryFlagPath = uploadBLC.GetFilePath(e.MobileCountry.Iso + LeillaKeys.PNG, LeillaKeys.AllCountriesFlags),
                    MobileNumber = e.MobileNumber,
                    Address = e.Address,
                    IsActive = e.IsActive,
                    JoiningDate = e.JoiningDate,
                    JobTitleName = e.JobTitle.Name,
                    ScheduleName = e.Schedule.Name,
                    EmployeeNumber = e.EmployeeNumber,
                    AttendanceTypeName = TranslationHelper.GetTranslation(e.AttendanceType.ToString(), requestInfo.Lang),
                    EmployeeTypeName = TranslationHelper.GetTranslation(e.EmployeeType.ToString(), requestInfo.Lang),
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees),
                    ProfileImageName = e.ProfileImageName,
                    DisableReason = e.DisableReason,
                    AllowFingerprintOutsideAllowedZones = e.AllowFingerprintOutsideAllowedZones,
                    AllowChangeFingerprintMobileCode = e.AllowChangeFingerprintMobileCode,
                    Zones = e.Zones
                    .Select(d => d.Zone.Name)
                    .ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            return employee;
        }
        public async Task<GetCurrentEmployeeInfoResponseModel> GetCurrentEmployeeInfo()
        {
            var isArabic = requestInfo?.Lang == LeillaKeys.Ar;

            var employeeId = await repositoryManager.UserRepository.Get(u => !u.IsDeleted && u.Id == requestInfo.UserId && u.EmployeeId != null)
                .Select(u => u.EmployeeId)
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);

            var employee = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(e => new GetCurrentEmployeeInfoResponseModel
                {
                    EmployeeNumber = e.EmployeeNumber,
                    Name = e.Name,
                    DapartmentName = e.Department.Name,
                    DirectManagerName = e.DirectManager.Name,
                    Email = e.Email,
                    MobileCountryCode = LeillaKeys.PlusSign + LeillaKeys.Space + e.MobileCountry.Dial,
                    MobileCountryName = isArabic ? e.MobileCountry.NameAr : e.MobileCountry.NameEn,
                    MobileNumber = e.MobileNumber,
                    Address = e.Address,
                    JobTitleName = e.JobTitle.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            return employee;
        }
        public async Task<GetEmployeeByIdResponseModel> GetById(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.Get(e => e.Id == employeeId && !e.IsDeleted)
                .Select(e => new GetEmployeeByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    DepartmentId = e.DepartmentId,
                    DirectManagerId = e.DirectManagerId,
                    Email = e.Email,
                    MobileCountryId = e.MobileCountryId,
                    MobileNumber = e.MobileNumber,
                    Address = e.Address,
                    IsActive = e.IsActive,
                    JoiningDate = e.JoiningDate,
                    JobTitleId = e.JobTitleId,
                    ScheduleId = e.ScheduleId,
                    AttendanceType = e.AttendanceType,
                    EmployeeType = e.EmployeeType,
                    AllowFingerprintOutsideAllowedZones = e.AllowFingerprintOutsideAllowedZones,
                    EmployeeNumber = e.EmployeeNumber,
                    ProfileImageName = e.ProfileImageName,
                    AllowChangeFingerprintMobileCode = e.AllowChangeFingerprintMobileCode,
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees),
                    DisableReason = e.DisableReason,
                    ZoneIds = e.Zones
                    .Join(repositoryManager.ZoneRepository.GetAll(),
                    zoneDepartment => zoneDepartment.ZoneId,
                    zone => zone.Id,
                    (zoneDepartment, zone) => zone.Id)
                    .ToList(),
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            return employee;
        }
        public async Task<bool> Enable(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == employeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);
            employee.Enable();

            #region Enable Related user
            var user = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.EmployeeId == employeeId);
            if(user != null)
            user.IsActive = true;
            #endregion
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);

            employee.Disable(model.DisableReason);
            #region Disable Related user
            var user = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.EmployeeId == model.Id);
            user.DisableReason = "Employee Disabled";
            user.IsActive = false;
            #endregion
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == employeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);
            employee.Delete();
            #region Delete Related user
            var user = await repositoryManager.UserRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.EmployeeId == employeeId);
            user.IsDeleted = true;
            #endregion
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetEmployeesInformationsResponseDTO> GetEmployeesInformations()
        {
            var employeeRepository = repositoryManager.EmployeeRepository;
            var employeeAttendanceRepository = repositoryManager.EmployeeAttendanceRepository;

            var currentCompanyId = requestInfo.CompanyId;
            var clientLocalDateTime = requestInfo.LocalDateTime;
            var clientLocalDate = clientLocalDateTime.Date;
            var clientLocalDateWeekDay = (WeekDay)clientLocalDateTime.DayOfWeek;
            var clientLocalTimeOnly = TimeOnly.FromTimeSpan(clientLocalDateTime.TimeOfDay);

            var query = employeeRepository.Get(employee => !employee.IsDeleted && employee.CompanyId == currentCompanyId);


            var dayTotalAbsencesCount = await employeeRepository.Get(employee => !employee.IsDeleted &&
             employee.CompanyId == currentCompanyId &&
             employee.ScheduleId != null &&
             employee.Schedule.ScheduleDays != null &&
             employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) != null &&
             employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift != null &&
             clientLocalDateTime.TimeOfDay > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime &&
             (employee.EmployeeAttendances == null || employee.EmployeeAttendances.FirstOrDefault(e => !e.IsDeleted && e.LocalDate.Date == clientLocalDate) == null))
            .CountAsync();


            var dayTotalVacationsCount = await repositoryManager.RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
            c.Request.CompanyId == currentCompanyId &&
            clientLocalDate >= c.Request.Date && clientLocalDate <= c.DateTo)
                .CountAsync() + await employeeRepository.Get(employee => !employee.IsDeleted &&
            employee.CompanyId == currentCompanyId &&
            employee.ScheduleId != null &&
            employee.Schedule.ScheduleDays != null &&
            employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay) == null)
                .CountAsync();

            #region Handle Response

            return new GetEmployeesInformationsResponseDTO
            {
                TotalCount = await query.CountAsync(),
                TotalAttendances = await employeeAttendanceRepository.Get(c => !c.IsDeleted && c.CompanyId == currentCompanyId &&
                c.EmployeeAttendanceChecks.Count() > 0 &&
                c.LocalDate.Date == clientLocalDate).CountAsync(),
                TotalAbsences = dayTotalAbsencesCount,
                TotalVacations = dayTotalVacationsCount
            };

            #endregion
        }
        public async Task<MemoryStream> ExportDraft()
        {
            #region  Get All Country with Name and IOS3
            var coutries = await repositoryManager.CountryRepository.Get(c => !c.IsDeleted && c.IsActive).Select(prop => new { prop.NameEn, prop.Iso3 }).ToListAsync();
            #endregion
            EmptyExcelDraftModelDTO employeeHeaderDraftDTO = new();
            employeeHeaderDraftDTO.FileName = AmgadKeys.EmployeeEmptyDraft;
            employeeHeaderDraftDTO.Obj = new EmployeeHeaderDraftDTO();
            employeeHeaderDraftDTO.ReadMeObj = coutries;
            employeeHeaderDraftDTO.ExcelExportScreen = ExcelExportScreen.Employees;

            return ExcelManager.ExportEmptyDraft(employeeHeaderDraftDTO);
        }
        public async Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile)
        {
            #region Fill IniValidationModelDTO
            IniValidationModelDTO iniValidationModelDTO = new();
            iniValidationModelDTO.FileStream = importedFile;
            iniValidationModelDTO.MaxRowCount = await repositoryManager.CompanyRepository.Get(c => c.Id == requestInfo.CompanyId).Select(cc => cc.NumberOfEmployees).FirstOrDefaultAsync()
                                               - await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId).Select(ee => ee.Id).CountAsync(); // will be configured
            iniValidationModelDTO.ColumnIndexToCheckNull.AddRange(new int[] { 1, 2, 7, 8, 14 });//employee Number & Name & Email && Mobile Number & MobileCode
            iniValidationModelDTO.ExcelExportScreen = ExcelExportScreen.Employees;
            string[] ExpectedHeaders = typeof(EmployeeHeaderDraftDTO).GetProperties().Select(prop => prop.Name).ToArray();
            iniValidationModelDTO.ExpectedHeaders = ExpectedHeaders;
            iniValidationModelDTO.Lang = requestInfo?.Lang;
            iniValidationModelDTO.ColumnsToCheckDuplication.AddRange(new int[] { 1, 2, 7, 8 });//employee Number & Name & Email & Mobile Number
            #endregion

            Dictionary<string, string> result = new();
            var validationMessages = ExcelValidator.InitialValidate(iniValidationModelDTO);
            if (validationMessages.Count > 0)
            {
                foreach (var kvp in validationMessages)
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }
            else
            {
                List<Employee> ImportedList = new();
                int EmployeeNumber;
                DateTime JoiningDate;
                bool IsActive;
                string[] zoneNames;
                Employee Temp = new();
                using var workbook = new XLWorkbook(iniValidationModelDTO.FileStream);
                var worksheet = workbook.Worksheet(1);
                var getNextCode = await repositoryManager.EmployeeRepository
               .Get(e => e.CompanyId == requestInfo.CompanyId && !e.IsDeleted)
               .Select(e => e.Code)
               .DefaultIfEmpty()
               .MaxAsync();

                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    //EGY for egypt
                    var foundCountryInDB = await repositoryManager.CountryRepository.
                        Get(e => !e.IsDeleted && e.Iso3 == row.Cell(14).GetString()).FirstOrDefaultAsync();
                    if (foundCountryInDB == null) //validate on Mobile Number code
                    {
                        result.Add(AmgadKeys.MissingData, TranslationHelper.
                            GetTranslation(AmgadKeys.SorryMobileCountryCodeNotValidOrNotExistPleaseSeeInstructions, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }

                    if (row.Cell(8).GetString().Trim().Length != foundCountryInDB.PhoneLength + 1)
                    {
                        result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(AmgadKeys.SorryTheMobileLenghtOfCountry, requestInfo?.Lang) + LeillaKeys.Space + requestInfo?.Lang == "ar" ? foundCountryInDB.NameAr : foundCountryInDB.NameEn + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.MustBe, requestInfo?.Lang) + LeillaKeys.Space + (foundCountryInDB.PhoneLength + 1) + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }

                    #region Validate Joining Date
                    if (row.Cell(10).GetString().Trim() == string.Empty)
                    {
                        JoiningDate = DateTime.MinValue;
                    }

                    else if (DateTime.TryParse(row.Cell(10).GetString().Trim(), out JoiningDate))
                    {

                    }
                    else
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.SorryTheJoiningDateNotValidDate, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                    #endregion
                    #region Validate IsActive 
                    if (row.Cell(13).GetString().Trim() == string.Empty)
                    {
                        IsActive = false;
                    }

                    else if (bool.TryParse(row.Cell(13).GetString().Trim(), out IsActive))
                    {

                    }
                    else
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.SorryIsActiveNotValidBoolean, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                    #endregion
                    #region Validate Employee Number 
                    if (int.TryParse(row.Cell(1).GetString().Trim(), out EmployeeNumber))
                    {

                    }
                    else
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.SorryEmployeeNumberNotValidAcceptOnlyNumber, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                    #endregion
                    #region Validate Employee Type 
                    if (row.Cell(12).GetString().Trim() != "Military" && row.Cell(12).GetString().Trim() != "CivilService" && row.Cell(12).GetString().Trim() != "Contract" && row.Cell(12).GetString().Trim() != "Contract" && row.Cell(12).GetString().Trim() != "ContractFromCompany")
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.SorryEmployeeTypeNotValidPleaseFollowTheInsructionToInsertItCorrectly, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }

                    #endregion
                    #region Validate Attendance Type 
                    if (row.Cell(11).GetString().Trim() != "FullAttendance" && row.Cell(11).GetString().Trim() != "PartialAttendance" && row.Cell(11).GetString().Trim() != "FreeOrShiftAttendance")
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.SorryAttendanceTypeNotValidPleaseFollowTheInsructionToInsertItCorrectly, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                    #endregion
                    #region Map Zones
                    zoneNames = row.Cell(15).GetString().Trim().Split(",");
                    Temp = new();
                    Temp.Zones = new List<ZoneEmployee>();
                    for (int i = 0; i < zoneNames.Count(); i++)
                    {
                        var foundZoneDb = await repositoryManager.ZoneRepository.Get(z => !z.IsDeleted && z.IsActive && z.Name == zoneNames[i].Trim()).FirstOrDefaultAsync();
                        if (foundZoneDb != null)
                        {
                            Temp.Zones.Add(new ZoneEmployee
                            {
                                ZoneId = foundZoneDb.Id
                            });
                        }
                        else
                        {
                            result.Add(AmgadKeys.MissingData, zoneNames[i].Trim() + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.SorryZoneNotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }
                    }
                    #endregion


                    var foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.EmployeeNumber == int.Parse(row.Cell(1).GetString())).FirstOrDefaultAsync();
                    if (foundEmployeeInDB == null) // employee number not found
                    {

                        foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Name == row.Cell(2).GetString()).FirstOrDefaultAsync();
                        if (foundEmployeeInDB == null) // Name Not Found
                        {
                            foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.MobileNumber == row.Cell(8).GetString()).FirstOrDefaultAsync();
                            if (foundEmployeeInDB == null) // mobile Number Not Found
                            {
                                foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Email == row.Cell(7).GetString()).FirstOrDefaultAsync();
                                if (foundEmployeeInDB == null) // Email Not Found
                                {
                                    getNextCode++;
                                    Temp.Code = getNextCode;
                                    Temp.AddedApplicationType = ApplicationType.Web;
                                    Temp.EmployeeNumber = int.Parse(row.Cell(1).GetString());
                                    Temp.Name = row.Cell(2).GetString().Trim();
                                    Temp.DepartmentId = repositoryManager.DepartmentRepository.Get(d => d.IsActive && !d.IsDeleted && d.Name == row.Cell(3).GetString().Trim()).Select(e => e.Id).FirstOrDefault();
                                    Temp.JobTitleId = repositoryManager.JobTitleRepository.Get(j => j.IsActive && !j.IsDeleted && j.CompanyId == requestInfo.CompanyId && j.Name == row.Cell(4).GetString().Trim()).Select(e => e.Id).FirstOrDefault();
                                    Temp.ScheduleId = repositoryManager.ScheduleRepository.Get(s => s.IsActive && !s.IsDeleted && s.CompanyId == requestInfo.CompanyId && s.Name == row.Cell(5).GetString().Trim()).Select(e => e.Id).FirstOrDefault();
                                    Temp.DirectManagerId = repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.IsActive && e.CompanyId == requestInfo.CompanyId && e.Name == row.Cell(6).GetString().Trim()).Select(e => e.Id).FirstOrDefault();
                                    Temp.Email = row.Cell(7).GetString();
                                    Temp.MobileNumber = row.Cell(8).GetString().Trim();
                                    Temp.Address = row.Cell(9).GetString().Trim();
                                    Temp.JoiningDate = JoiningDate;
                                    Temp.AttendanceType = row.Cell(11).GetString() == "FullAttendance" ? AttendanceType.FullAttendance : row.Cell(11).GetString() == "PartialAttendance" ? AttendanceType.PartialAttendance : row.Cell(11).GetString() == "FreeOrShiftAttendance" ? AttendanceType.FreeOrShiftAttendance : AttendanceType.FullAttendance;
                                    Temp.EmployeeType = row.Cell(12).GetString() == "Military" ? EmployeeType.Military : row.Cell(8).GetString() == "CivilService" ? EmployeeType.CivilService : row.Cell(8).GetString() == "Contract" ? EmployeeType.Military : row.Cell(8).GetString() == "ContractFromCompany" ? EmployeeType.ContractFromCompany : EmployeeType.Military;
                                    Temp.IsActive = IsActive;
                                    Temp.CompanyId = requestInfo.CompanyId;
                                    Temp.AddedDate = DateTime.Now;
                                    Temp.AddUserId = requestInfo.UserId;
                                    Temp.MobileCountryId = foundCountryInDB.Id;
                                    Temp.InsertedFromExcel = true;
                                    if (Temp.DepartmentId == 0)
                                    {
                                        result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(AmgadKeys.ThisDepartment, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.NotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                        return result;
                                    }
                                    else if (Temp.JobTitleId == 0)
                                    {
                                        result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(AmgadKeys.ThisJobTitle, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.NotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                        return result;
                                    }
                                    else if (Temp.ScheduleId == 0)
                                    {
                                        result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(AmgadKeys.ThisSchedule, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.NotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                        return result;
                                    }
                                    else if (Temp.DirectManagerId == 0)
                                    {
                                        result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(AmgadKeys.ThisDirectManager, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.NotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                        return result;
                                    }

                                    else
                                    {
                                        ImportedList.Add(Temp);
                                    }
                                }
                                else
                                {
                                    result.Add(AmgadKeys.DuplicationInDBProblem, foundEmployeeInDB.Email + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.ThisEmailIsUsedByEmployee, requestInfo?.Lang) + LeillaKeys.Space + foundEmployeeInDB.Name + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                    return result;
                                }
                            }
                            else
                            {
                                result.Add(AmgadKeys.DuplicationInDBProblem, foundEmployeeInDB.MobileNumber + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.ThisMobileNumberIsUsedByEmployee, requestInfo?.Lang) + LeillaKeys.Space + foundEmployeeInDB.Name + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                return result;
                            }
                        }
                        else
                        {
                            result.Add(AmgadKeys.DuplicationInDBProblem, foundEmployeeInDB.Name + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.ThisNameIsUsedByEmployee, requestInfo?.Lang) + LeillaKeys.Space + foundEmployeeInDB.Name + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }
                    }
                    else
                    {
                        result.Add(AmgadKeys.DuplicationInDBProblem, foundEmployeeInDB.EmployeeNumber + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.ThisEmployeeNumberIsUsedByEmployee, requestInfo?.Lang) + LeillaKeys.Space + foundEmployeeInDB.Name + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                }
                repositoryManager.EmployeeRepository.BulkInsert(ImportedList);
                await unitOfWork.SaveAsync();
                result.Add(AmgadKeys.Success, TranslationHelper.GetTranslation(AmgadKeys.ImportedSuccessfully, requestInfo?.Lang) + LeillaKeys.Space + ImportedList.Count + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.EmployeeEnteredSuccessfully, requestInfo?.Lang));
            }
            return result;
        }
        public async Task<bool> UpdateSpecificDataForEmployee(UpdateSpecificModelDTO model)
        {
            unitOfWork.CreateTransaction();
            #region Upload Profile Image
            string UserimageName = null;
            string EmployeeimageName = null;
            if (model.ProfileImageFile != null && model.ProfileImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Employees)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
                EmployeeimageName = result.FileName;
                //var userResult = await uploadBLC.UploadFile(model.ProfileImageFile, LeillaKeys.Users)
                //   ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadProfileImage);
                //UserimageName = userResult.FileName;

            }
            #endregion
            #region Update Employee
            var getEmployee = await repositoryManager.EmployeeRepository
                .GetEntityByConditionWithTrackingAsync(employee => !employee.IsDeleted && employee.IsActive
            && employee.Id == requestInfo.EmployeeId);
            getEmployee.ModifiedDate = DateTime.UtcNow;
            getEmployee.ModifyUserId = requestInfo.UserId;
            getEmployee.Address = model.Address;
            getEmployee.ProfileImageName = !string.IsNullOrEmpty(EmployeeimageName) ? EmployeeimageName : !string.IsNullOrEmpty(model.ProfileImageName)
                ? getEmployee.ProfileImageName : null;
            getEmployee.ModifiedApplicationType = requestInfo.ApplicationType;

            var getUser = await repositoryManager.UserRepository
             .GetEntityByConditionWithTrackingAsync(employee => !employee.IsDeleted && employee.IsActive
           && employee.EmployeeId == requestInfo.EmployeeId && employee.Id == requestInfo.User.Id);
            getUser.ProfileImageName = getEmployee.ProfileImageName;
            getUser.ModifiedApplicationType = requestInfo.ApplicationType;
            getUser.ModifiedDate = DateTime.UtcNow;
            getUser.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion
            #region Handle Response
            await unitOfWork.CommitAsync();
            return true;
            #endregion
        }

        public async Task<GetEmployeesSchedulePlanResponse> GetCurrentEmployeeShedulePlanInPeriod(GetEmployeeSchedulePlanCritria criteria)
        {

            criteria.EmployeeId = requestInfo.EmployeeId ?? 0;
            criteria.DateFrom = DateTime.UtcNow;
            //criteria.DateTo = criteria.DateFrom.AddDays(30);
            criteria.DateTo = criteria.DateFrom.AddDays(100);

            // Retrieve the schedule plans for the specified employee within the current month
            // Retrieve the schedule plans for the specified employee within the current month
            var employeeSchedulePlans = await repositoryManager.EmployeeRepository.Get(sp =>
                sp.Id == criteria.EmployeeId &&
                !sp.IsDeleted &&
                sp.IsActive &&
                sp.CompanyId == requestInfo.CompanyId &&
                sp.SchedulePlanEmployees.Any(spe =>
                    spe.SchedulePlan.DateFrom.Date >= criteria.DateFrom && // Start of the month
                    spe.SchedulePlan.DateFrom.Date <= criteria.DateTo     // End of the month
                )
            ).Select(employee => new
            {
                Employee = employee,
                SchedulePlans = employee.SchedulePlanEmployees
                    .Where(spe =>
                        spe.SchedulePlan.DateFrom.Date >= criteria.DateFrom && // Start of the month
                        spe.SchedulePlan.DateFrom.Date <= criteria.DateTo     // End of the month
                    )
                    .OrderBy(spe => spe.SchedulePlan.DateFrom)
                    .Select(spe => new
                    {
                        SchedulePlan = spe.SchedulePlan,
                        Schedule = spe.SchedulePlan.Schedule, // Include the Schedule object
                                                              // CheckInTime = spe.CheckInTime, // Adjust property names as needed
                                                              // CheckOutTime = spe.CheckOutTime // Adjust property names as needed
                    })
            }).ToListAsync();

            // Extract and return the projected employee details along with associated schedule plans
            var projectedEmployeeDetails = employeeSchedulePlans.SelectMany(item => item.SchedulePlans.Select(schedulePlan =>
                new GetCurrentEmployeeScheduleInPeriodDTO
                {
                    Name = item.Employee.Name, // Replace with actual property path
                    DayName = schedulePlan.SchedulePlan.DateFrom.DayOfWeek.ToString(),
                    Date = schedulePlan.SchedulePlan.DateFrom,
                    ScheduleName = schedulePlan.Schedule.Name, // Replace with actual property path
                                                               // CheckInTime = schedulePlan.SchedulePlan.Schedule.ScheduleDays..CheckInTime,
                                                               // CheckOutTime = schedulePlan.CheckOutTime
                })).OrderBy(scheduleDetail => scheduleDetail.Date).ToList();

            #region Handle Response
            return new GetEmployeesSchedulePlanResponse
            {
                EmployeeSchedulePlan = projectedEmployeeDetails,
                TotalCount = projectedEmployeeDetails.Count()
            };
            #endregion
            // return new GetEmployeesSchedulePlanResponse();
        }

        // Helper method to get the end date of the current schedule plan
        DateTime GetEndDate(List<SchedulePlan> lSchedulPlan, SchedulePlan currentPlan, DateTime overallEndDate)
        {
            // Get the next schedule plan after the current one
            var nextPlan = lSchedulPlan.FirstOrDefault(sp => sp.DateFrom > currentPlan.DateFrom);

            // If there's a next plan, return its start date - 1 day
            if (nextPlan != null)
            {
                return nextPlan.DateFrom.AddDays(-1);
            }

            // If there's no next plan, return the overall end date
            return overallEndDate;
        }
        public async Task<GetEmployeesForDropDownResponse> GetForDropDownEmployeeNotHaveUser(GetEmployeesCriteria criteria)
        {
            criteria.IsFreeEmployee = true;
            criteria.IsActive = true;
            var employeeRepository = repositoryManager.EmployeeRepository;
            var query = employeeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = employeeRepository.OrderBy(query, nameof(Employee.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var employeesList = await queryPaged.Select(e => new GetEmployeesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetEmployeesForDropDownResponse
            {
                Employees = employeesList,
                TotalCount = await query.CountAsync()
            };

            #endregion
        }



    }






}

