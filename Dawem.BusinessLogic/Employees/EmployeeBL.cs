using AutoMapper;
using ClosedXML.Excel;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Excel;
using Dawem.Models.Dtos.Excel.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Employees;
using Dawem.Translations;
using Dawem.Validation.BusinessValidation.ExcelValidations;
using Dawem.Validation.FluentValidation.Employees.Employees;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IEmployeeBLValidation employeeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        public EmployeeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           IEmployeeBLValidation _employeeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            employeeBLValidation = _employeeBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
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
            getEmployee.ScheduleId = model.ScheduleId;
            getEmployee.EmployeeNumber = model.EmployeeNumber;
            getEmployee.AnnualVacationBalance = model.AnnualVacationBalance;
            getEmployee.Email = model.Email;
            getEmployee.AllowChangeFingerprintMobileCode = model.AllowChangeFingerprintMobileCode;
            getEmployee.MobileNumber = model.MobileNumber;
            getEmployee.Address = model.Address;
            getEmployee.ProfileImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.ProfileImageName)
                ? getEmployee.ProfileImageName : null;
            getEmployee.ModifiedApplicationType = requestInfo.ApplicationType;

            #region Update ZoneEmployee

            List<ZoneEmployee> existDbList = repositoryManager.ZoneEmployeeRepository
                    .GetByCondition(e => e.EmployeeId == getEmployee.Id)
                    .ToList();

            List<int> existingZoneIds = existDbList.Select(e => e.ZoneId).ToList();

            var addedEmployeeZones = model.Zones != null ? model.Zones
                .Where(ge => !existingZoneIds.Contains(ge.ZoneId))
                .Select(ge => new ZoneEmployee
                {
                    EmployeeId = model.Id,
                    ZoneId = ge.ZoneId,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<ZoneEmployee>();

            List<int> ZonesToRemove = existDbList
                .Where(ge => model.ZoneIds == null || !model.ZoneIds.Contains(ge.ZoneId))
                .Select(ge => ge.ZoneId)
                .ToList();

            List<ZoneEmployee> removedEmployeeZones = repositoryManager.ZoneEmployeeRepository
                .GetByCondition(e => e.EmployeeId == model.Id && ZonesToRemove.Contains(e.ZoneId))
                .ToList();

            if (removedEmployeeZones.Count > 0)
                repositoryManager.ZoneEmployeeRepository.BulkDeleteIfExist(removedEmployeeZones);
            if (addedEmployeeZones.Count > 0)
                repositoryManager.ZoneEmployeeRepository.BulkInsert(addedEmployeeZones);

            #endregion

            await unitOfWork.SaveAsync();

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
                AnnualVacationBalance = e.AnnualVacationBalance,
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
                    AnnualVacationBalance = e.AnnualVacationBalance,
                    JobTitleName = e.JobTitle.Name,
                    ScheduleName = e.Schedule.Name,
                    EmployeeNumber = e.EmployeeNumber,
                    AttendanceTypeName = TranslationHelper.GetTranslation(e.AttendanceType.ToString(), requestInfo.Lang),
                    EmployeeTypeName = TranslationHelper.GetTranslation(e.EmployeeType.ToString(), requestInfo.Lang),
                    ProfileImagePath = uploadBLC.GetFilePath(e.ProfileImageName, LeillaKeys.Employees),
                    ProfileImageName = e.ProfileImageName,
                    DisableReason = e.DisableReason,
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
                    AnnualVacationBalance = e.AnnualVacationBalance,
                    JobTitleId = e.JobTitleId,
                    ScheduleId = e.ScheduleId,
                    AttendanceType = e.AttendanceType,
                    EmployeeType = e.EmployeeType,
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
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);
            employee.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int employeeId)
        {
            var employee = await repositoryManager.EmployeeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == employeeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeNotFound);
            employee.Delete();
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
             clientLocalTimeOnly > employee.Schedule.ScheduleDays.FirstOrDefault(d => !d.IsDeleted && d.WeekDay == clientLocalDateWeekDay).Shift.CheckInTime &&
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
            EmptyExcelDraftModelDTO employeeHeaderDraftDTO = new();
            employeeHeaderDraftDTO.FileName = AmgadKeys.EmployeeEmptyDraft;
            employeeHeaderDraftDTO.Obj = new EmployeeHeaderDraftDTO();
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
            iniValidationModelDTO.ColumnIndexToCheckNull.AddRange(new int[] { 1, 2, 7 });//employee Number & Name & Email
            iniValidationModelDTO.ExcelExportScreen = ExcelExportScreen.Employees;

            string[] ExpectedHeaders = { "EmployeeNumber", "EmployeeName", "DepartmentName", "JobTitle"
                                        , "ScheduleName",
                                         "DirectManagerName","Email","MobileNumber","Address","JoiningDate",
                                         "AttendanceType","EmployeeType","AnnualVacationBalance","IsActive"};
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
                Employee Temp = new();
                using var workbook = new XLWorkbook(iniValidationModelDTO.FileStream);
                var worksheet = workbook.Worksheet(1);
                var getNextCode = await repositoryManager.EmployeeRepository
               .Get(e => e.CompanyId == requestInfo.CompanyId)
               .Select(e => e.Code)
               .DefaultIfEmpty()
               .MaxAsync();
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    var foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.EmployeeNumber == int.Parse(row.Cell(1).GetString())).FirstOrDefaultAsync();
                    if (foundEmployeeInDB == null) // employee number not found
                    {
                        foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Name == (row.Cell(2).GetString())).FirstOrDefaultAsync();
                        if (foundEmployeeInDB == null) // Name Not Found
                        {
                            foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.MobileNumber == (row.Cell(8).GetString())).FirstOrDefaultAsync();
                            if (foundEmployeeInDB == null) // mobile Number Not Found
                            {
                                foundEmployeeInDB = await repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Email == (row.Cell(7).GetString())).FirstOrDefaultAsync();
                                if (foundEmployeeInDB == null) // Email Not Found
                                {

                                    Temp = new();
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
                                    Temp.MobileNumber = row.Cell(8).GetString();
                                    Temp.Address = row.Cell(9).GetString();
                                    Temp.JoiningDate = DateTime.Parse(row.Cell(10).GetString());
                                    Temp.AttendanceType = row.Cell(11).GetString() == "FullAttendance" ? AttendanceType.FullAttendance : row.Cell(11).GetString() == "PartialAttendance" ? AttendanceType.PartialAttendance : row.Cell(11).GetString() == "FreeOrShiftAttendance" ? AttendanceType.FreeOrShiftAttendance : AttendanceType.FullAttendance;
                                    Temp.EmployeeType = row.Cell(12).GetString() == "Military" ? EmployeeType.Military : row.Cell(8).GetString() == "CivilService" ? EmployeeType.CivilService : row.Cell(8).GetString() == "Contract" ? EmployeeType.Military : row.Cell(8).GetString() == "ContractFromCompany" ? EmployeeType.ContractFromCompany : EmployeeType.Military;
                                    Temp.AnnualVacationBalance = int.Parse(row.Cell(13).GetString());
                                    Temp.IsActive = bool.Parse(row.Cell(14).GetString());
                                    Temp.CompanyId = requestInfo.CompanyId;
                                    Temp.AddedDate = DateTime.Now;
                                    Temp.AddUserId = requestInfo.UserId;
                                    Temp.MobileCountryId = 65;
                                    Temp.InsertedFromExcel = true;
                                    if (Temp.DepartmentId == 0)
                                    {
                                        result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(AmgadKeys.ThisDepartment, requestInfo?.Lang)  + LeillaKeys.Space +TranslationHelper.GetTranslation(AmgadKeys.NotFound , requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber , requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
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
                                    else if (Temp.AnnualVacationBalance < 0)
                                    {
                                        result.Add(AmgadKeys.WrongData, TranslationHelper.GetTranslation(AmgadKeys.AnnualVacationBalanceCanNotBeNegativeValue , requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                                        return result;
                                    }
                                    else
                                    {
                                        ImportedList.Add(Temp);
                                    }
                                }
                                else
                                {
                                    result.Add(AmgadKeys.DuplicationInDBProblem, foundEmployeeInDB.Email + LeillaKeys.Space + TranslationHelper.GetTranslation( AmgadKeys.ThisEmailIsUsedByEmployee , requestInfo?.Lang) + LeillaKeys.Space + foundEmployeeInDB.Name + LeillaKeys.Space +TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber,requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
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
                result.Add(AmgadKeys.Success, TranslationHelper.GetTranslation(AmgadKeys.ImportedSuccessfully , requestInfo?.Lang) + LeillaKeys.Space + ImportedList.Count + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.EmployeeEnteredSuccessfully,requestInfo?.Lang));
            }
            return result;
        }
    }
}

