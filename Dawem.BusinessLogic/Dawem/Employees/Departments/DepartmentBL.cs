using AutoMapper;
using ClosedXML.Excel;
using Dawem.Contract.BusinessLogic.Dawem.Employees.Department;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Departments;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Excel;
using Dawem.Models.Dtos.Dawem.Excel.Departments;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Employees.Departments;
using Dawem.Translations;
using Dawem.Validation.BusinessValidation.Dawem.ExcelValidations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Employees.Departments
{
    public class DepartmentBL : IDepartmentBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDepartmentBLValidation departmentBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;

        public DepartmentBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IUploadBLC _uploadBLC,

           IDepartmentBLValidation _departmentBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            departmentBLValidation = _departmentBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;

        }
        public async Task<int> Create(CreateDepartmentModel model)
        {
            #region Business Validation

            await departmentBLValidation.CreateValidation(model);

            #endregion

            #region assign Delegatos In DepartmentZones Object
            if (model.ZoneIds != null && model.ZoneIds.Count > 0)
                model.MapDepartmentZones();
            #endregion

            #region assign DelegatorsIdes In DepartmentManagerDelegators Object
            if (model.ManagerDelegatorIds != null && model.ManagerDelegatorIds.Count > 0)
                model.MapDepartmentManagarDelegators();
            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Department

            #region Set Department code
            var getNextCode = await repositoryManager.DepartmentRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var department = mapper.Map<Department>(model);
            department.CompanyId = requestInfo.CompanyId;
            department.AddUserId = requestInfo.UserId;
            department.AddedApplicationType = requestInfo.ApplicationType;
            department.Code = getNextCode;
            repositoryManager.DepartmentRepository.Insert(department);

            /*var department2 = mapper.Map<Department>(model);
            department2.CompanyId = requestInfo.CompanyId;
            department2.AddUserId = requestInfo.UserId;
            department2.AddedApplicationType = requestInfo.ApplicationType;
            department2.Code = getNextCode;

            repositoryManager.DepartmentRepository.Insert(department2);*/

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return department.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDepartmentModel model)
        {
            #region assign Delegatos In DepartmentZones Object
            model.MapDepartmentZones();
            #endregion

            #region assign DelegatorsIdes In DepartmentManagerDelegators Object
            model.MapDepartmentManagarDelegators();
            #endregion

            #region Business Validation

            await departmentBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Department

            var getDepartment = await repositoryManager.DepartmentRepository.GetEntityByConditionWithTrackingAsync(department => !department.IsDeleted
            && department.Id == model.Id);

            if (getDepartment != null)
            {
                getDepartment.Name = model.Name;
                getDepartment.ParentId = model.ParentId;
                getDepartment.IsActive = model.IsActive;
                getDepartment.ModifiedDate = DateTime.Now;
                getDepartment.ModifyUserId = requestInfo.UserId;
                getDepartment.ManagerId = model.ManagerId;
                getDepartment.Notes = model.Notes;

                #region Update ZoneDepartment

                List<ZoneDepartment> existDbList = repositoryManager.ZoneDepartmentRepository
                                .GetByCondition(e => e.DepartmentId == getDepartment.Id)
                                .ToList();

                List<int> existingZoneIds = existDbList.Select(e => e.ZoneId).ToList();

                var addedDepartmentZones = model.Zones != null ? model.Zones
                    .Where(ge => !existingZoneIds.Contains(ge.ZoneId))
                    .Select(ge => new ZoneDepartment
                    {
                        DepartmentId = model.Id,
                        ZoneId = ge.ZoneId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    }).ToList() : new List<ZoneDepartment>();

                List<int> ZonesToRemove = existDbList
                    .Where(ge => model.ZoneIds == null || !model.ZoneIds.Contains(ge.ZoneId))
                    .Select(ge => ge.ZoneId)
                    .ToList();

                List<ZoneDepartment> removedDepartmentZones = repositoryManager.ZoneDepartmentRepository
                    .GetByCondition(e => e.DepartmentId == model.Id && ZonesToRemove.Contains(e.ZoneId))
                    .ToList();

                if (removedDepartmentZones.Count > 0)
                    repositoryManager.ZoneDepartmentRepository.BulkDeleteIfExist(removedDepartmentZones);
                if (addedDepartmentZones.Count > 0)
                    repositoryManager.ZoneDepartmentRepository.BulkInsert(addedDepartmentZones);

                #endregion

                #region Update DepartmentManagerDelgators

                List<DepartmentManagerDelegator> ExistDbList = repositoryManager.DepartmentManagerDelegatorRepository
                    .GetByCondition(e => e.DepartmentId == getDepartment.Id)
                    .ToList();

                List<int> existingEmployeeIds = ExistDbList.Select(e => e.EmployeeId).ToList();

                List<DepartmentManagerDelegator> addedDepartmentManagerDelegators = model.ManagerDelegators
                    .Where(gmd => !existingEmployeeIds.Contains(gmd.EmployeeId))
                    .Select(gmd => new DepartmentManagerDelegator
                    {
                        DepartmentId = model.Id,
                        EmployeeId = gmd.EmployeeId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    }).ToList();

                List<int> DepartmentManagerDelegatorToRemove = ExistDbList
                    .Where(gmd => !model.ManagerDelegatorIds.Contains(gmd.EmployeeId))
                    .Select(gmd => gmd.EmployeeId)
                    .ToList();

                List<DepartmentManagerDelegator> removedDepartmentManagerDelegators = repositoryManager.DepartmentManagerDelegatorRepository
                    .GetByCondition(e => e.DepartmentId == model.Id && DepartmentManagerDelegatorToRemove.Contains(e.EmployeeId))
                    .ToList();
                if (removedDepartmentManagerDelegators.Count > 0)
                    repositoryManager.DepartmentManagerDelegatorRepository.BulkDeleteIfExist(removedDepartmentManagerDelegators);
                if (addedDepartmentManagerDelegators.Count > 0)
                    repositoryManager.DepartmentManagerDelegatorRepository.BulkInsert(addedDepartmentManagerDelegators);

                #endregion

                await unitOfWork.SaveAsync();

                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);


        }
        public async Task<GetDepartmentsResponse> Get(GetDepartmentsCriteria criteria)
        {
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.GetAsQueryable(criteria);
            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(Department.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(dep => new GetDepartmentsResponseModel
            {
                Id = dep.Id,
                Code = dep.Code,
                Name = dep.Name,
                //Notes = dep.Notes,
                NumberOfEmployees = dep.Employees != null ? dep.Employees.Count : 0,
                IsActive = dep.IsActive,
                Manager = dep.ManagerId != null ? new DepartmentManagarForGridDTO
                {
                    ManagerName = dep.Manager.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(dep.Manager.ProfileImageName, LeillaKeys.Employees),
                } : null
            }).ToListAsync();

            return new GetDepartmentsResponse
            {
                Departments = departmentsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetDepartmentsForDropDownResponse> GetForDropDown(GetDepartmentsCriteria criteria)
        {
            criteria.IsActive = true;
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = departmentRepository.OrderBy(query, nameof(Department.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetDepartmentsForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDepartmentsForDropDownResponse
            {
                Departments = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDepartmentsForTreeResponse> GetForTree(GetDepartmentsForTreeCriteria criteria)
        {
            criteria.IsActive = true;
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = departmentRepository.OrderBy(query, nameof(Department.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var departmentsList = await queryPaged.Select(e => new GetDepartmentsForTreeResponseModel
            {
                Id = e.Id,
                Name = e.Name,
                HasChildren = e.Children.Any(c => !c.IsDeleted && c.IsActive),
                ChildrenCount = e.Children.Count(c => !c.IsDeleted && c.IsActive)
            }).ToListAsync();

            return new GetDepartmentsForTreeResponse
            {
                Departments = departmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDepartmentInfoResponseModel> GetInfo(int departmentId)
        {
            var department = await repositoryManager.DepartmentRepository.Get(e => e.Id == departmentId && !e.IsDeleted)
                .Select(dep => new GetDepartmentInfoResponseModel
                {
                    Code = dep.Code,
                    Name = dep.Name,
                    ParentName = dep.Parent != null ? dep.Parent.Name : null,
                    IsActive = dep.IsActive,
                    Notes = dep.Notes,
                    ManagerDelegators = dep.ManagerDelegators
                    .Select(d => d.Employee.Name)
                    .ToList(),
                    Manager = dep.Manager.Name,
                    Zones = dep.Zones
                    .Select(d => d.Zone.Name)
                    .ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);

            return department;
        }
        public async Task<GetDepartmentByIdResponseModel> GetById(int DepartmentId)
        {
            var department = await repositoryManager.DepartmentRepository.Get(e => e.Id == DepartmentId && !e.IsDeleted)
                .Select(dep => new GetDepartmentByIdResponseModel
                {
                    Id = dep.Id,
                    Code = dep.Code,
                    Name = dep.Name,
                    ParentId = dep.Parent != null ? dep.ParentId : null,
                    IsActive = dep.IsActive,
                    Notes = dep.Notes,
                    ManagerDelegatorIds = dep.ManagerDelegators
                    .Select(d => d.EmployeeId)
                    .ToList(),
                    ZoneIds = dep.Zones
                    .Select(z => z.ZoneId)
                    .ToList(),
                    ManagerId = dep.Manager.Id
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);

            return department;

        }
        public async Task<bool> Delete(int departmentd)
        {
            var department = await repositoryManager.DepartmentRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == departmentd) ??
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);
            department.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Enable(int departmentId)
        {
            var department = await repositoryManager.DepartmentRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == departmentId) ??
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);
            department.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DepartmentRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetDepartmentsInformationsResponseDTO> GetDepartmentsInformations()
        {
            var departmentRepository = repositoryManager.DepartmentRepository;
            var query = departmentRepository.Get(department => department.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetDepartmentsInformationsResponseDTO
            {
                TotalCount = await query.Where(department => !department.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(department => !department.IsDeleted && department.IsActive).CountAsync(),
                NotActiveCount = await query.Where(department => !department.IsDeleted && !department.IsActive).CountAsync(),
                DeletedCount = await query.Where(department => department.IsDeleted).CountAsync()
            };

            #endregion
        }

        public async Task<MemoryStream> ExportDraft()
        {
            EmptyExcelDraftModelDTO departmentHeaderDraftDTO = new();
            departmentHeaderDraftDTO.FileName = AmgadKeys.DepartmentEmptyDraft;
            departmentHeaderDraftDTO.Obj = new DepartmentHeaderDraftDTO();
            departmentHeaderDraftDTO.ExcelExportScreen = ExcelExportScreen.Departments;
            return ExcelManager.ExportEmptyDraft(departmentHeaderDraftDTO);
        }

        public async Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile)
        {
            #region Fill IniValidationModelDTO
            IniValidationModelDTO iniValidationModelDTO = new();
            iniValidationModelDTO.FileStream = importedFile;
            iniValidationModelDTO.MaxRowCount = 0;
            iniValidationModelDTO.ColumnIndexToCheckNull.AddRange(new int[] { 1 });//department Name can't be null
            iniValidationModelDTO.ExcelExportScreen = ExcelExportScreen.Departments;
            iniValidationModelDTO.ExpectedHeaders = typeof(DepartmentHeaderDraftDTO).GetProperties().Select(prop => prop.Name).ToArray();
            iniValidationModelDTO.Lang = requestInfo?.Lang;
            iniValidationModelDTO.ColumnsToCheckDuplication.AddRange(new int[] { 1 });//department Name can't be duplicated
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
                List<Department> ImportedList = new();
                Department Temp = new();
                using var workbook = new XLWorkbook(iniValidationModelDTO.FileStream);
                var worksheet = workbook.Worksheet(1);
                bool IsActive;
                string[] zoneNames;
                string[] delegatorNames;

                var getNextCode = await repositoryManager.DepartmentRepository
               .Get(e => e.CompanyId == requestInfo.CompanyId && !e.IsDeleted)
               .Select(e => e.Code)
               .DefaultIfEmpty()
               .MaxAsync();

                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    #region Validate IsActive 
                    if (row.Cell(4).GetString().Trim() == string.Empty)
                    {
                        IsActive = false;
                    }

                    else if (bool.TryParse(row.Cell(4).GetString().Trim(), out IsActive))
                    {

                    }
                    else
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.SorryIsActiveNotValidBoolean, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                    #endregion
                    #region Map Zones
                    zoneNames = row.Cell(5).GetString().Trim().Split(",");
                    Temp = new();
                    Temp.Zones = new List<ZoneDepartment>();
                    for (int i = 0; i < zoneNames.Count(); i++)
                    {
                       
                        var foundZoneDb = await repositoryManager.ZoneRepository.Get(z => !z.IsDeleted && z.IsActive && z.Name == zoneNames[i].Trim()).FirstOrDefaultAsync();
                        if (foundZoneDb != null)
                        {
                            Temp.Zones.Add(new ZoneDepartment
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
                    #region Map Delegators
                    delegatorNames = row.Cell(6).GetString().Trim().Split(",");
                    Temp.ManagerDelegators = new List<DepartmentManagerDelegator>();

                    for (int i = 0; i < delegatorNames.Count(); i++)
                    {
                        var foundEmpDb =await repositoryManager.EmployeeRepository.Get(z => !z.IsDeleted && z.IsActive && z.Name == delegatorNames[i].Trim()).FirstOrDefaultAsync();
                        if (foundEmpDb != null)
                        {
                            Temp.ManagerDelegators.Add(new DepartmentManagerDelegator
                            {
                                EmployeeId = foundEmpDb.Id
                            });
                        }
                        else
                        {
                            result.Add(AmgadKeys.MissingData, zoneNames[i].Trim() + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.SorryZoneNotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }
                    }
                    #endregion
                    var foundDepartmentInDB = await repositoryManager.DepartmentRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Name == row.Cell(1).GetString().Trim()).FirstOrDefaultAsync();
                    if (foundDepartmentInDB == null) // Department Name not found
                    {
                        getNextCode++;
                        Temp.Code = getNextCode;
                        Temp.AddedApplicationType = ApplicationType.Web;
                        Temp.Name = row.Cell(1).GetString().Trim();
                        Temp.ManagerId = repositoryManager.EmployeeRepository.Get(e => !e.IsDeleted && e.IsActive && e.CompanyId == requestInfo.CompanyId && e.Name == row.Cell(3).GetString().Trim()).Select(e => e.Id).FirstOrDefault();
                        Temp.IsActive = IsActive;
                        Temp.ParentId = repositoryManager.DepartmentRepository.Get(e => !e.IsDeleted && e.IsActive && e.CompanyId == requestInfo.CompanyId && e.Name == row.Cell(2).GetString().Trim()).Select(e => e.Id).FirstOrDefault();
                        Temp.CompanyId = requestInfo.CompanyId;
                        Temp.AddedDate = DateTime.Now;
                        Temp.AddUserId = requestInfo.UserId;
                        Temp.InsertedFromExcel = true;
                        if (Temp.ManagerId == 0)
                        {
                            result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(AmgadKeys.SorryThisEmployeeNotFound, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }
                        else if (Temp.ParentId == 0)
                        {
                            result.Add(AmgadKeys.MissingData, TranslationHelper.GetTranslation(LeillaKeys.SorryDepartmentNotFound, requestInfo.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }
                        else
                        {
                            ImportedList.Add(Temp);
                        }
                    }
                    else
                    {
                        result.Add(AmgadKeys.DuplicationInDBProblem, foundDepartmentInDB.Name + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.ThisDepartmentIsUsedBefore, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                }
                repositoryManager.DepartmentRepository.BulkInsert(ImportedList);
                await unitOfWork.SaveAsync();
                result.Add(AmgadKeys.Success, TranslationHelper.GetTranslation(AmgadKeys.ImportedSuccessfully, requestInfo.Lang) + LeillaKeys.Space + ImportedList.Count + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.DepartmentEnteredSuccessfully, requestInfo?.Lang));
            }
            return result;
        }
    }
}

