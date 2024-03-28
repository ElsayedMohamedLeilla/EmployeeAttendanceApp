using AutoMapper;
using ClosedXML.Excel;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.Zones;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Excel;
using Dawem.Models.Dtos.Dawem.Excel.Zones;
using Dawem.Models.Generic.Exceptions;
using Dawem.Models.Response.Core.Zones;
using Dawem.Translations;
using Dawem.Validation.BusinessValidation.Dawem.ExcelValidations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.Zones
{
    public class ZoneBL : IZoneBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IZoneBLValidation ZoneBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;


        public ZoneBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
         IUploadBLC _uploadBLC,
        IZoneBLValidation _ZoneBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            ZoneBLValidation = _ZoneBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;

        }
        public async Task<int> Create(CreateZoneDTO model)
        {
            #region Business Validation
            await ZoneBLValidation.CreateValidation(model);
            #endregion
            unitOfWork.CreateTransaction();
            #region Insert Zone
            #region Set Zone code
            var getNextCode = await repositoryManager.ZoneRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion
            var Zone = mapper.Map<Zone>(model);
            Zone.CompanyId = requestInfo.CompanyId;
            Zone.AddUserId = requestInfo.UserId;
            Zone.AddedApplicationType = requestInfo.ApplicationType;
            Zone.Code = getNextCode;
            repositoryManager.ZoneRepository.Insert(Zone);
            await unitOfWork.SaveAsync();
            #endregion
            #region Handle Response
            await unitOfWork.CommitAsync();
            return Zone.Id;
            #endregion

        }
        public async Task<bool> Update(UpdateZoneDTO model)
        {
            #region Business Validation

            await ZoneBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Zone

            var getZone = await repositoryManager.ZoneRepository.GetEntityByConditionWithTrackingAsync(grp => !grp.IsDeleted
            && grp.Id == model.Id);

            if (getZone != null)
            {
                getZone.Name = model.Name;
                getZone.IsActive = model.IsActive;
                getZone.Latitude = model.Latitude;
                getZone.Longitude = model.Longitude;
                getZone.Radius = model.Radius;
                getZone.ModifiedDate = DateTime.UtcNow;
                getZone.ModifyUserId = requestInfo.UserId;
                getZone.ModifiedApplicationType = requestInfo.ApplicationType;
            }
            else
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            #endregion

            await unitOfWork.SaveAsync();

            #region Handle Response

            await unitOfWork.CommitAsync();

            return true;

            #endregion

        }
        public async Task<GetZoneResponseDTO> Get(GetZoneCriteria criteria)
        {
            var ZoneRepository = repositoryManager.ZoneRepository;
            var query = ZoneRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ZoneRepository.OrderBy(query, nameof(Zone.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response
            var ZonesList = await queryPaged.Select(Zone => new GetZoneResponseModelDTO
            {
                Id = Zone.Id,
                Code = Zone.Code,
                Name = Zone.Name,
                IsActive = Zone.IsActive,
                Latitude = Zone.Latitude,
                Longitude = Zone.Longitude,
                Radius = Zone.Radius
            }).ToListAsync();



            return new GetZoneResponseDTO
            {
                Zones = ZonesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetZoneDropDownResponseDTO> GetForDropDown(GetZoneCriteria criteria)
        {
            criteria.IsActive = true;
            var ZoneRepository = repositoryManager.ZoneRepository;
            var query = ZoneRepository.GetAsQueryable(criteria);
            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ZoneRepository.OrderBy(query, nameof(Zone.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion
            #region Handle Response

            var ZonesList = await queryPaged.Select(e => new GetZoneForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetZoneDropDownResponseDTO
            {
                Zones = ZonesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetZoneInfoResponseDTO> GetInfo(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository.Get(e => e.Id == ZoneId && !e.IsDeleted)
           .Select(Zone => new GetZoneInfoResponseDTO
           {
               Code = Zone.Code,
               Name = Zone.Name,
               IsActive = Zone.IsActive,
               Radius = Zone.Radius,
               Latitude = Zone.Latitude,
               Longitude = Zone.Longitude,
           }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            return Zone;
        }
        public async Task<GetZoneByIdResponseDTO> GetById(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository.Get(e => e.Id == ZoneId && !e.IsDeleted)
                .Select(zone => new GetZoneByIdResponseDTO
                {
                    Id = zone.Id,
                    Name = zone.Name,
                    IsActive = zone.IsActive,
                    Longitude = zone.Longitude,
                    Latitude = zone.Latitude,
                    Radius = zone.Radius
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            return Zone;

        }
        public async Task<bool> Delete(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == ZoneId) ??
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);

            Zone.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int ZoneId)
        {
            var Zone = await repositoryManager.ZoneRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == ZoneId) ??
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);
            Zone.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var Zone = await repositoryManager.ZoneRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryZoneNotFound);
            Zone.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetZonesInformationsResponseDTO> GetZonesInformations()
        {
            var zoneRepository = repositoryManager.ZoneRepository;
            var query = zoneRepository.Get(zone => zone.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetZonesInformationsResponseDTO
            {
                TotalCount = await query.Where(zone => !zone.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(zone => !zone.IsDeleted && zone.IsActive).CountAsync(),
                NotActiveCount = await query.Where(zone => !zone.IsDeleted && !zone.IsActive).CountAsync(),
                DeletedCount = await query.Where(zone => zone.IsDeleted).CountAsync()
            };

            #endregion
        }

        public async Task<MemoryStream> ExportDraft()
        {
            EmptyExcelDraftModelDTO zoneHeaderDraftDTO = new();
            zoneHeaderDraftDTO.FileName = AmgadKeys.ZoneEmptyDraft;
            zoneHeaderDraftDTO.Obj = new ZoneHeaderDraftDTO();
            zoneHeaderDraftDTO.ExcelExportScreen = ExcelExportScreen.Zones;
            return ExcelManager.ExportEmptyDraft(zoneHeaderDraftDTO);
        }

        public async Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile)
        {
            #region Fill IniValidationModelDTO
            IniValidationModelDTO iniValidationModelDTO = new();
            iniValidationModelDTO.FileStream = importedFile;
            iniValidationModelDTO.MaxRowCount = 0;
            iniValidationModelDTO.ColumnIndexToCheckNull.AddRange(new int[] { 1, 2, 3 });//Zone Name Lat Long can't be null
            iniValidationModelDTO.ExcelExportScreen = ExcelExportScreen.Zones;
            string[] ExpectedHeaders = { "ZoneName", "Latitude", "Longitude", "Radius", "IsActive" };
            iniValidationModelDTO.ExpectedHeaders = ExpectedHeaders;
            iniValidationModelDTO.Lang = requestInfo?.Lang;
            iniValidationModelDTO.ColumnsToCheckDuplication.AddRange(new int[] { 1, 2, 3 });//Zone Name lat long  can't be duplicated
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
                List<Zone> ImportedList = new();
                Zone Temp = new();
                using var workbook = new XLWorkbook(iniValidationModelDTO.FileStream);
                var worksheet = workbook.Worksheet(1);
                var getNextCode = await repositoryManager.EmployeeRepository
               .Get(e => e.CompanyId == requestInfo.CompanyId)
               .Select(e => e.Code)
               .DefaultIfEmpty()
               .MaxAsync();
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    #region Check Valid Lat Long Reduis
                    double tempLatitude;
                    double tempLongtude;
                    double tempRaduis;

                    Temp = new();
                    // check if the enterted value is valid double for lat long radius
                    if (double.TryParse(row.Cell(2).GetString().Trim(), out tempLatitude))
                    {
                        if (double.TryParse(row.Cell(3).GetString().Trim(), out tempLongtude))
                        {
                            if (double.TryParse(row.Cell(4).GetString().Trim(), out tempRaduis))
                            {

                            }
                            else
                            {
                                result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.ThisRaduisNotValid + LeillaKeys.Space + AmgadKeys.NotFound + LeillaKeys.Space + AmgadKeys.OnRowNumber + LeillaKeys.Space + row.RowNumber(), requestInfo?.Lang));
                                return result;
                            }
                        }
                        else
                        {
                            result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.ThisLongtudeNotValid, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }

                    }
                    else
                    {
                        result.Add(AmgadKeys.MissMatchDataType, TranslationHelper.GetTranslation(AmgadKeys.ThisLatitudeNotValid, requestInfo?.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                    #endregion

                    var foundZoneInDB = await repositoryManager.ZoneRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Name == row.Cell(1).GetString().Trim()).FirstOrDefaultAsync();
                    if (foundZoneInDB == null) // Name not found
                    {
                        foundZoneInDB = await repositoryManager.ZoneRepository.Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Latitude == tempLatitude && e.Longitude == tempLongtude && e.Radius == tempRaduis).FirstOrDefaultAsync();
                        if (foundZoneInDB == null)
                        {
                            getNextCode++;
                            Temp.Code = getNextCode;
                            Temp.AddedApplicationType = ApplicationType.Web;
                            Temp.Name = row.Cell(1).GetString().Trim();
                            Temp.Latitude = tempLatitude;
                            Temp.Longitude = tempLongtude;
                            Temp.Radius = tempRaduis;
                            Temp.IsActive = bool.Parse(row.Cell(5).GetString());
                            Temp.CompanyId = requestInfo.CompanyId;
                            Temp.AddedDate = DateTime.Now;
                            Temp.AddUserId = requestInfo.UserId;
                            Temp.InsertedFromExcel = true;
                            ImportedList.Add(Temp);
                        }
                        else
                        {
                            result.Add(AmgadKeys.DuplicationInDBProblem, TranslationHelper.GetTranslation(AmgadKeys.TheSameLatitudeLongtudeRaduisIsUsedBy, requestInfo?.Lang) + LeillaKeys.Space + foundZoneInDB.Name + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo?.Lang) + LeillaKeys.Space + row.RowNumber());
                            return result;
                        }
                    }
                    else
                    {
                        result.Add(AmgadKeys.DuplicationInDBProblem, TranslationHelper.GetTranslation(AmgadKeys.SorryZoneNameIsDuplicated, requestInfo.Lang) + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.OnRowNumber, requestInfo.Lang) + LeillaKeys.Space + row.RowNumber());
                        return result;
                    }
                }
                repositoryManager.ZoneRepository.BulkInsert(ImportedList);
                await unitOfWork.SaveAsync();
                result.Add(AmgadKeys.Success, TranslationHelper.GetTranslation(AmgadKeys.ImportedSuccessfully, requestInfo?.Lang) + LeillaKeys.Space + ImportedList.Count + LeillaKeys.Space + TranslationHelper.GetTranslation(AmgadKeys.ZoneEnteredSuccessfully, requestInfo?.Lang));
            }
            return result;
        }
    }
}
