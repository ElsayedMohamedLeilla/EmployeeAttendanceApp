using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Providers;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.Dtos.Dawem.Providers.Companies;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Providers.Companies;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Dawem.Providers;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Provider
{
    public class CompanyBL : ICompanyBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ICompanyBLValidation companyBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;
        public CompanyBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext,
           ICompanyBLValidation _companyBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            companyBLValidation = _companyBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;
        }
        public async Task<int> Create(CreateCompanyModel model)
        {
            #region Model Validation

            var createCompanyModel = new CreateCompanyModelValidator();
            var createCompanyModelResult = createCompanyModel.Validate(model);
            if (!createCompanyModelResult.IsValid)
            {
                var error = createCompanyModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await companyBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Logo Image

            string imageName = null;
            if (model.LogoImageFile != null && model.LogoImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.LogoImageFile, LeillaKeys.Companies)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadLogoImage);
                imageName = result.FileName;
            }

            #endregion

            #region Upload Files

            List<string> fileNames = null;

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                fileNames = new List<string>();

                foreach (var attachment in model.Attachments)
                {
                    if (attachment != null && attachment.Length > 0)
                    {
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.Companies)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadCompanyAttachements);
                        fileNames.Add(result.FileName);
                    }
                }
                model.AttachmentsNames = fileNames;
            }

            #endregion

            #region Insert Company

            #region Set Company code

            var getNextCode = await repositoryManager.CompanyRepository
                .Get()
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var company = mapper.Map<Company>(model);
            company.AddUserId = requestInfo.UserId;
            company.AddedApplicationType = requestInfo.ApplicationType;
            company.LogoImageName = imageName;
            company.Code = getNextCode;
            repositoryManager.CompanyRepository.Insert(company);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return company.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateCompanyModel model)
        {
            #region Model Validation

            var companyId = requestInfo.CompanyId;
            var updateCompanyModelValidator = new UpdateCompanyModelValidator();
            var updateCompanyModelValidatorResult = updateCompanyModelValidator.Validate(model);
            if (!updateCompanyModelValidatorResult.IsValid)
            {
                var error = updateCompanyModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await companyBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Logo Image

            string imageName = null;
            if (model.LogoImageFile != null && model.LogoImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.LogoImageFile, LeillaKeys.Companies)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadLogoImage);
                imageName = result.FileName;
            }

            #endregion

            #region Upload Files

            var newFileNames = new List<string>();

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                newFileNames = new List<string>();

                foreach (var attachment in model.Attachments)
                {
                    if (attachment != null && attachment.Length > 0)
                    {
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.Companies)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadCompanyAttachements);
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Company

            var getCompany = await repositoryManager.CompanyRepository
                .GetEntityByConditionWithTrackingAsync(company => !company.IsDeleted
            && company.Id == companyId);
            getCompany.ModifiedDate = DateTime.Now;
            getCompany.ModifyUserId = requestInfo.UserId;
            getCompany.Email = model.Email;
            getCompany.PreferredLanguageId = model.PreferredLanguageId;
            getCompany.WebSite = model.WebSite;
            getCompany.HeadquarterAddress = model.HeadquarterAddress;
            getCompany.HeadquarterLocationLatitude = model.HeadquarterLocationLatitude;
            getCompany.HeadquarterLocationLongtude = model.HeadquarterLocationLongitude;
            getCompany.HeadquarterPostalCode = model.HeadquarterPostalCode;
            getCompany.TotalNumberOfEmployees = model.TotalNumberOfEmployees;
            getCompany.ImportDefaultData = model.ImportDefaultData;
            getCompany.LogoImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.LogoImageName)
                ? getCompany.LogoImageName : null;
            getCompany.ModifiedApplicationType = requestInfo.ApplicationType;

            await unitOfWork.SaveAsync();

            #region Handle Update Industries

            var existIndustriesDbList = await repositoryManager.CompanyIndustryRepository
                    .Get(e => e.CompanyId == getCompany.Id)
                    .ToListAsync();

            var existingIndustriesIds = existIndustriesDbList.Select(e => e.Id)
                .ToList();

            var addedCompanyIndustries = model.Industries != null ? model.Industries
                .Where(ge => !existingIndustriesIds.Contains(ge.Id))
                .Select(ge => new CompanyIndustry
                {
                    CompanyId = companyId,
                    Name = ge.Name,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<CompanyIndustry>();

            var removedCompanyIndustriesIds = existIndustriesDbList
                .Where(ge => model.Industries == null || !model.Industries.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedCompanyIndustries = existIndustriesDbList
                .Where(e => removedCompanyIndustriesIds.Contains(e.Id))
                .ToList();

            var updatedCompanyIndustries = existIndustriesDbList.
                Where(i => model.Industries != null && model.Industries.Any(mi => mi.Id == i.Id && mi.Name != i.Name)).
                ToList();

            if (removedCompanyIndustries.Count > 0)
                repositoryManager.CompanyIndustryRepository.BulkDeleteIfExist(removedCompanyIndustries);
            if (addedCompanyIndustries.Count > 0)
                repositoryManager.CompanyIndustryRepository.BulkInsert(addedCompanyIndustries);
            if (updatedCompanyIndustries.Count > 0)
            {
                updatedCompanyIndustries.ForEach(i =>
                {
                    i.Name = model.Industries.FirstOrDefault(mi => mi.Id == i.Id)?.Name;
                });
                repositoryManager.CompanyIndustryRepository.BulkUpdate(updatedCompanyIndustries);
            }

            #endregion

            #region Handle Update Branches

            var existBranchesDbList = await repositoryManager.CompanyBranchRepository
                    .Get(e => e.CompanyId == getCompany.Id)
                    .ToListAsync();

            var existingBranchesIds = existBranchesDbList.Select(e => e.Id)
                .ToList();

            var addedCompanyBranches = model.Branches != null ? model.Branches
                .Where(ge => !existingBranchesIds.Contains(ge.Id))
                .Select(ge => new CompanyBranch
                {
                    CompanyId = companyId,
                    Name = ge.Name,
                    Address = ge.Address,
                    Latitude = ge.Latitude,
                    Longitude = ge.Longitude,
                    PostalCode = ge.PostalCode,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<CompanyBranch>();

            var removedCompanyBranchesIds = existBranchesDbList
                .Where(ge => model.Branches == null || !model.Branches.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedCompanyBranches = existBranchesDbList
                .Where(e => removedCompanyBranchesIds.Contains(e.Id))
                .ToList();

            var updatedCompanyBranches = existBranchesDbList.
                Where(i => model.Branches != null && model.Branches.Any(mi => mi.Id == i.Id &&
                (mi.Name != i.Name || mi.Address != i.Address || mi.Latitude != i.Latitude ||
                mi.Longitude != i.Longitude || mi.PostalCode != i.PostalCode))).
                ToList();

            if (removedCompanyBranches.Count > 0)
                repositoryManager.CompanyBranchRepository.BulkDeleteIfExist(removedCompanyBranches);
            if (addedCompanyBranches.Count > 0)
                repositoryManager.CompanyBranchRepository.BulkInsert(addedCompanyBranches);
            if (updatedCompanyBranches.Count > 0)
            {
                updatedCompanyBranches.ForEach(i =>
                {
                    i.Name = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Name;
                    i.Address = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Address;
                    i.Latitude = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Latitude;
                    i.Longitude = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Longitude;
                    i.PostalCode = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.PostalCode;
                });
                repositoryManager.CompanyBranchRepository.BulkUpdate(updatedCompanyBranches);
            }

            #endregion

            #region Update Attachements 

            var existAttachementsDbList = await repositoryManager.CompanyAttachmentRepository
                    .Get(e => e.CompanyId == getCompany.Id)
                    .ToListAsync();

            var existingFileNames = existAttachementsDbList.Select(e => e.FileName).ToList();

            var addedAttachements = newFileNames
                .Select(fileName => new CompanyAttachment
                {
                    CompanyId = getCompany.Id,
                    FileName = fileName,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();

            var removedFileNames = existAttachementsDbList
                .Where(ge => model.AttachmentsNames == null || !model.AttachmentsNames.Contains(ge.FileName))
                .Select(ge => ge.FileName)
                .ToList();

            var removedAttachments = await repositoryManager.CompanyAttachmentRepository
                .Get(e => e.CompanyId == getCompany.Id && removedFileNames.Contains(e.FileName))
                .ToListAsync();

            if (removedAttachments.Count > 0)
                repositoryManager.CompanyAttachmentRepository.BulkDeleteIfExist(removedAttachments);
            if (addedAttachements.Count > 0)
                repositoryManager.CompanyAttachmentRepository.BulkInsert(addedAttachements);

            #endregion

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<bool> Update(AdminPanelUpdateCompanyModel model)
        {
            #region Model Validation

            var updateCompanyModelValidator = new AdminPanelUpdateCompanyModelValidator();
            var updateCompanyModelValidatorResult = updateCompanyModelValidator.Validate(model);
            if (!updateCompanyModelValidatorResult.IsValid)
            {
                var error = updateCompanyModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await companyBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Upload Logo Image

            string imageName = null;
            if (model.LogoImageFile != null && model.LogoImageFile.Length > 0)
            {
                var result = await uploadBLC.UploadFile(model.LogoImageFile, LeillaKeys.Companies)
                    ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadLogoImage);
                imageName = result.FileName;
            }

            #endregion

            #region Upload Files

            var newFileNames = new List<string>();

            if (model.Attachments != null && model.Attachments.Count > 0)
            {
                newFileNames = new List<string>();

                foreach (var attachment in model.Attachments)
                {
                    if (attachment != null && attachment.Length > 0)
                    {
                        var result = await uploadBLC.UploadFile(attachment, LeillaKeys.Companies)
                            ?? throw new BusinessValidationException(LeillaKeys.SorryErrorHappenWhileUploadCompanyAttachements);
                        newFileNames.Add(result.FileName);
                    }
                }
            }

            #endregion

            #region Update Company

            var getCompany = await repositoryManager.CompanyRepository
                .GetEntityByConditionWithTrackingAsync(company => !company.IsDeleted
            && company.Id == model.Id);
            getCompany.ModifiedDate = DateTime.Now;
            getCompany.ModifyUserId = requestInfo.UserId;
            getCompany.Email = model.Email;
            getCompany.PreferredLanguageId = model.PreferredLanguageId;
            getCompany.WebSite = model.WebSite;
            getCompany.HeadquarterAddress = model.HeadquarterAddress;
            getCompany.HeadquarterLocationLatitude = model.HeadquarterLocationLatitude;
            getCompany.HeadquarterLocationLongtude = model.HeadquarterLocationLongitude;
            getCompany.HeadquarterPostalCode = model.HeadquarterPostalCode;
            getCompany.TotalNumberOfEmployees = model.TotalNumberOfEmployees;
            getCompany.ImportDefaultData = model.ImportDefaultData;
            getCompany.LogoImageName = !string.IsNullOrEmpty(imageName) ? imageName : !string.IsNullOrEmpty(model.LogoImageName)
                ? getCompany.LogoImageName : null;
            getCompany.ModifiedApplicationType = requestInfo.ApplicationType;

            await unitOfWork.SaveAsync();

            #region Handle Update Industries

            var existIndustriesDbList = await repositoryManager.CompanyIndustryRepository
                    .Get(e => e.CompanyId == getCompany.Id)
                    .ToListAsync();

            var existingIndustriesIds = existIndustriesDbList.Select(e => e.Id)
                .ToList();

            var addedCompanyIndustries = model.Industries != null ? model.Industries
                .Where(ge => !existingIndustriesIds.Contains(ge.Id))
                .Select(ge => new CompanyIndustry
                {
                    CompanyId = model.Id,
                    Name = ge.Name,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<CompanyIndustry>();

            var removedCompanyIndustriesIds = existIndustriesDbList
                .Where(ge => model.Industries == null || !model.Industries.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedCompanyIndustries = existIndustriesDbList
                .Where(e => removedCompanyIndustriesIds.Contains(e.Id))
                .ToList();

            var updatedCompanyIndustries = existIndustriesDbList.
                Where(i => model.Industries != null && model.Industries.Any(mi => mi.Id == i.Id && mi.Name != i.Name)).
                ToList();

            if (removedCompanyIndustries.Count > 0)
                repositoryManager.CompanyIndustryRepository.BulkDeleteIfExist(removedCompanyIndustries);
            if (addedCompanyIndustries.Count > 0)
                repositoryManager.CompanyIndustryRepository.BulkInsert(addedCompanyIndustries);
            if (updatedCompanyIndustries.Count > 0)
            {
                updatedCompanyIndustries.ForEach(i =>
                {
                    i.Name = model.Industries.FirstOrDefault(mi => mi.Id == i.Id)?.Name;
                });
                repositoryManager.CompanyIndustryRepository.BulkUpdate(updatedCompanyIndustries);
            }

            #endregion

            #region Handle Update Branches

            var existBranchesDbList = await repositoryManager.CompanyBranchRepository
                    .Get(e => e.CompanyId == getCompany.Id)
                    .ToListAsync();

            var existingBranchesIds = existBranchesDbList.Select(e => e.Id)
                .ToList();

            var addedCompanyBranches = model.Branches != null ? model.Branches
                .Where(ge => !existingBranchesIds.Contains(ge.Id))
                .Select(ge => new CompanyBranch
                {
                    CompanyId = model.Id,
                    Name = ge.Name,
                    Address = ge.Address,
                    Latitude = ge.Latitude,
                    Longitude = ge.Longitude,
                    PostalCode = ge.PostalCode,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<CompanyBranch>();

            var removedCompanyBranchesIds = existBranchesDbList
                .Where(ge => model.Branches == null || !model.Branches.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedCompanyBranches = existBranchesDbList
                .Where(e => removedCompanyBranchesIds.Contains(e.Id))
                .ToList();

            var updatedCompanyBranches = existBranchesDbList.
                Where(i => model.Branches != null && model.Branches.Any(mi => mi.Id == i.Id &&
                (mi.Name != i.Name || mi.Address != i.Address || mi.Latitude != i.Latitude 
                || mi.Longitude != i.Longitude || mi.PostalCode != i.PostalCode))).
                ToList();

            if (removedCompanyBranches.Count > 0)
                repositoryManager.CompanyBranchRepository.BulkDeleteIfExist(removedCompanyBranches);
            if (addedCompanyBranches.Count > 0)
                repositoryManager.CompanyBranchRepository.BulkInsert(addedCompanyBranches);
            if (updatedCompanyBranches.Count > 0)
            {
                updatedCompanyBranches.ForEach(i =>
                {
                    i.Name = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Name;
                    i.Address = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Address;
                    i.Latitude = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Latitude;
                    i.Longitude = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.Longitude;
                    i.PostalCode = model.Branches.FirstOrDefault(mi => mi.Id == i.Id)?.PostalCode;
                });
                repositoryManager.CompanyBranchRepository.BulkUpdate(updatedCompanyBranches);
            }

            #endregion

            #region Update Attachements 

            var existAttachementsDbList = await repositoryManager.CompanyAttachmentRepository
                    .Get(e => e.CompanyId == getCompany.Id)
                    .ToListAsync();

            var existingFileNames = existAttachementsDbList.Select(e => e.FileName).ToList();

            var addedAttachements = newFileNames
                .Select(fileName => new CompanyAttachment
                {
                    CompanyId = getCompany.Id,
                    FileName = fileName,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();

            var removedFileNames = existAttachementsDbList
                .Where(ge => model.AttachmentsNames == null || !model.AttachmentsNames.Contains(ge.FileName))
                .Select(ge => ge.FileName)
                .ToList();

            var removedAttachments = await repositoryManager.CompanyAttachmentRepository
                .Get(e => e.CompanyId == getCompany.Id && removedFileNames.Contains(e.FileName))
                .ToListAsync();

            if (removedAttachments.Count > 0)
                repositoryManager.CompanyAttachmentRepository.BulkDeleteIfExist(removedAttachments);
            if (addedAttachements.Count > 0)
                repositoryManager.CompanyAttachmentRepository.BulkInsert(addedAttachements);

            #endregion

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetCompaniesResponse> Get(GetCompaniesCriteria criteria)
        {
            var companyRepository = repositoryManager.CompanyRepository;
            var query = companyRepository.GetAsQueryable(criteria);
            var isArabic = requestInfo.Lang == LeillaKeys.Ar;

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = companyRepository.OrderBy(query, nameof(Company.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var companiesList = await queryPaged.Select(company => new GetCompaniesResponseModel
            {
                Id = company.Id,
                Code = company.Code,
                Name = company.Name,
                CountryName = isArabic ? company.Country.NameAr : company.Country.NameEn,
                SubscriptionTypeName = TranslationHelper.
                GetTranslation(nameof(SubscriptionType) + LeillaKeys.Dash +
                (company.Subscription.Plan.IsTrial ? LeillaKeys.Trial : LeillaKeys.Subscription), requestInfo.Lang),
                IsActive = company.IsActive,
                NumberOfEmployees = company.NumberOfEmployees,
                LogoImagePath = uploadBLC.GetFilePath(company.LogoImageName, LeillaKeys.Companies)
            }).ToListAsync();

            return new GetCompaniesResponse
            {
                Companies = companiesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetCompaniesForDropDownResponse> GetForDropDown(GetCompaniesCriteria criteria)
        {
            criteria.IsActive = true;
            var companyRepository = repositoryManager.CompanyRepository;
            var query = companyRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = companyRepository.OrderBy(query, nameof(Company.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var companiesList = await queryPaged.Select(e => new GetCompaniesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetCompaniesForDropDownResponse
            {
                Companies = companiesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetCompanyInfoResponseModel> GetInfo(int companyId)
        {
            var isArabic = requestInfo.Lang == LeillaKeys.Ar;

            var company = await repositoryManager.CompanyRepository.Get(company => company.Id == companyId && !company.IsDeleted)
                .Select(company => new GetCompanyInfoResponseModel
                {
                    Code = company.Code,
                    Name = company.Name,
                    CountryName = isArabic ? company.Country.NameAr : company.Country.NameEn,
                    PreferredLanguageName = company.PreferredLanguage.NativeName,
                    Email = company.Email,
                    IdentityCode = company.IdentityCode,
                    WebSite = company.WebSite,
                    SubscriptionTypeName = TranslationHelper.
                    GetTranslation(LeillaKeys.SubscriptionType + LeillaKeys.Dash +
                    (company.Subscription.Plan.IsTrial ? LeillaKeys.Trial : LeillaKeys.Subscription), requestInfo.Lang),
                    HeadquarterAddress = company.HeadquarterAddress,
                    HeadquarterLocationLatitude = company.HeadquarterLocationLatitude,
                    HeadquarterLocationLongtude = company.HeadquarterLocationLongtude,
                    HeadquarterPostalCode = company.HeadquarterPostalCode,
                    NumberOfEmployees = company.NumberOfEmployees,
                    TotalNumberOfEmployees = company.TotalNumberOfEmployees,
                    NumberOfFingerprintDevices = company.FingerprintDevices.Count,
                    NumberOfUsers = company.Users.Count,
                    NumberOfZones = company.Zones.Count,
                    IsActive = company.IsActive,
                    Industries = company.CompanyIndustries
                    .Select(industry => industry.Name)
                    .ToList(),
                    Branches = company.CompanyBranches
                    .Select(branch => new CompanyBranchModel
                    {
                        Name = branch.Name,
                        Address = branch.Address,
                        Latitude = branch.Latitude,
                        Longitude = branch.Longitude,
                        PostalCode = branch.PostalCode,
                    }).ToList(),
                    Attachments = company.CompanyAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.AssignmentRequests),
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCompanyNotFound);

            return company;
        }
        public async Task<GetCompanyByIdResponseModel> GetById(int companyId)
        {
            var company = await repositoryManager.CompanyRepository.Get(company => company.Id == companyId && !company.IsDeleted)
                .Select(company => new GetCompanyByIdResponseModel
                {
                    Id = company.Id,
                    Code = company.Code,
                    Name = company.Name,
                    CountryId = company.CountryId,
                    PreferredLanguageId = company.PreferredLanguageId,
                    Email = company.Email,
                    IdentityCode = company.IdentityCode,
                    WebSite = company.WebSite,
                    HeadquarterAddress = company.HeadquarterAddress,
                    HeadquarterLocationLatitude = company.HeadquarterLocationLatitude,
                    HeadquarterLocationLongtude = company.HeadquarterLocationLongtude,
                    HeadquarterPostalCode = company.HeadquarterPostalCode,
                    NumberOfEmployees = company.NumberOfEmployees,
                    TotalNumberOfEmployees = company.TotalNumberOfEmployees,
                    LogoImageName = company.LogoImageName,
                    LogoImagePath = uploadBLC.GetFilePath(company.LogoImageName, LeillaKeys.Companies),
                    IsActive = company.IsActive,
                    Industries = company.CompanyIndustries
                    .Select(industry => new CompanyIndustryModel
                    {
                        Id = industry.Id,
                        Name = industry.Name
                    })
                    .ToList(),
                    Branches = company.CompanyBranches
                    .Select(branch => new CompanyBranchModel
                    {
                        Id = branch.Id,
                        Name = branch.Name,
                        Address = branch.Address,
                        Latitude = branch.Latitude,
                        Longitude = branch.Longitude,
                        PostalCode = branch.PostalCode,
                    }).ToList(),
                    Attachments = company.CompanyAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.AssignmentRequests),
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCompanyNotFound);

            return company;
        }
        public async Task<AdminPanelGetCompanyByIdResponseModel> GetById()
        {
            var companyId = requestInfo.CompanyId;

            var company = await repositoryManager.CompanyRepository.Get(company => company.Id == companyId && !company.IsDeleted)
                .Select(company => new AdminPanelGetCompanyByIdResponseModel
                {
                    Code = company.Code,
                    Name = company.Name,
                    CountryId = company.CountryId,
                    PreferredLanguageId = company.PreferredLanguageId,
                    Email = company.Email,
                    IdentityCode = company.IdentityCode,
                    WebSite = company.WebSite,
                    HeadquarterAddress = company.HeadquarterAddress,
                    HeadquarterLocationLatitude = company.HeadquarterLocationLatitude,
                    HeadquarterLocationLongtude = company.HeadquarterLocationLongtude,
                    HeadquarterPostalCode = company.HeadquarterPostalCode,
                    NumberOfEmployees = company.NumberOfEmployees,
                    TotalNumberOfEmployees = company.TotalNumberOfEmployees,
                    LogoImageName = company.LogoImageName,
                    LogoImagePath = uploadBLC.GetFilePath(company.LogoImageName, LeillaKeys.Companies),
                    IsActive = company.IsActive,
                    Industries = company.CompanyIndustries
                    .Select(industry => new CompanyIndustryModel
                    {
                        Id = industry.Id,
                        Name = industry.Name
                    })
                    .ToList(),
                    Branches = company.CompanyBranches
                    .Select(branch => new CompanyBranchModel
                    {
                        Id = branch.Id,
                        Name = branch.Name,
                        Address = branch.Address,
                        Latitude = branch.Latitude,
                        Longitude = branch.Longitude,
                        PostalCode = branch.PostalCode,
                    }).ToList(),
                    Attachments = company.CompanyAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, LeillaKeys.Companies),
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCompanyNotFound);

            return company;
        }
        public async Task<bool> Enable(int companyId)
        {
            var company = await repositoryManager.CompanyRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == companyId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCompanyNotFound);
            company.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var company = await repositoryManager.CompanyRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryCompanyNotFound);
            company.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int companyId)
        {
            var company = await repositoryManager.CompanyRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == companyId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCompanyNotFound);
            company.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetCompaniesInformationsResponseDTO> GetCompaniesInformations()
        {
            var companyRepository = repositoryManager.CompanyRepository;
            var query = companyRepository.Get();

            #region Handle Response

            return new GetCompaniesInformationsResponseDTO
            {
                TotalCount = await query.Where(company => !company.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(company => !company.IsDeleted && company.IsActive).CountAsync(),
                NotActiveCount = await query.Where(company => !company.IsDeleted && !company.IsActive).CountAsync(),
                DeletedCount = await query.Where(company => company.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

