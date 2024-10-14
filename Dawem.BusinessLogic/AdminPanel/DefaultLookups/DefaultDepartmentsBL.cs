using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultDepartments;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultDepartmentsBL : IDefaultDepartmentsBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultDepartmentsBLValidation DepartmentsBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultDepartmentsBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultDepartmentsBLValidation _DepartmentsBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            DepartmentsBLValidation = _DepartmentsBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultDepartmentsDTO model)
        {
            #region Business Validation

            await DepartmentsBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Departments

            #region Set Departments code

            var getNextCode = await repositoryManager.DefaultDepartmentsRepository
                .Get(e => e.LookupType == LookupsType.Departments)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var Departments = mapper.Map<DefaultLookup>(model);
            Departments.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            Departments.LookupType = LookupsType.Departments;
            Departments.AddUserId = requestInfo.UserId;
            Departments.Code = getNextCode;
            repositoryManager.DefaultDepartmentsRepository.Insert(Departments);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return Departments.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultDepartmentsDTO model)
        {
            #region Business Validation

            await DepartmentsBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Departments
            var getDepartments = await repositoryManager.DefaultDepartmentsRepository.GetByIdAsync(model.Id);
            getDepartments.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getDepartments.DefaultType = model.DefaultType;
            getDepartments.IsActive = model.IsActive;
            getDepartments.ModifiedDate = DateTime.Now;
            getDepartments.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getDepartments.Id)
                    .ToListAsync();

            var existingNameTranslationsIds = exisNameTranslationsDbList.Select(e => e.Id)
                .ToList();

            var addedPlanNameTranslations = model.NameTranslations != null ? model.NameTranslations
                .Where(ge => !existingNameTranslationsIds.Contains(ge.Id))
                .Select(ge => new DefaultLookupsNameTranslation
                {
                    DefaultLookupId = model.Id,
                    LanguageId = ge.LanguageId,
                    Name = ge.Name,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<DefaultLookupsNameTranslation>();

            var removedPlanNameTranslationsIds = exisNameTranslationsDbList
                .Where(ge => model.NameTranslations == null || !model.NameTranslations.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedPlanNameTranslations = exisNameTranslationsDbList
                .Where(e => removedPlanNameTranslationsIds.Contains(e.Id))
                .ToList();

            var updatedPlanNameTranslations = exisNameTranslationsDbList.
                Where(nt => model.NameTranslations != null && model.NameTranslations.
                Any(mi => mi.Id == nt.Id && (mi.Name != nt.Name || mi.LanguageId != nt.LanguageId))).
                ToList();

            if (removedPlanNameTranslations.Count() > 0)
                repositoryManager.DefaultLookupsNameTranslationRepository.BulkDeleteIfExist(removedPlanNameTranslations);
            if (addedPlanNameTranslations.Count() > 0)
                repositoryManager.DefaultLookupsNameTranslationRepository.BulkInsert(addedPlanNameTranslations);
            if (updatedPlanNameTranslations.Count() > 0)
            {
                var modelNameTranslationsDict = model.NameTranslations?.ToDictionary(mi => mi.Id, mi => mi);

                updatedPlanNameTranslations.ForEach(i =>
                {
                    if (modelNameTranslationsDict != null && modelNameTranslationsDict.TryGetValue(i.Id, out var translation))
                    {
                        i.Name = translation.Name;
                        i.LanguageId = translation.LanguageId;
                    }
                });

                repositoryManager.DefaultLookupsNameTranslationRepository.BulkUpdate(updatedPlanNameTranslations);
            }
            await unitOfWork.SaveAsync();


            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }


        public async Task<GetDefaultDepartmentsResponseDTO> Get(GetDefaultDepartmentsCriteria criteria)
        {
            var DepartmentsRepository = repositoryManager.DefaultDepartmentsRepository;
            var query = DepartmentsRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = DepartmentsRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var DepartmentsList = await queryPaged.Select(Departments => new GetDefaultDepartmentsResponseModelDTO
            {
                Id = Departments.Id,
                Code = Departments.Code,
                Name = Departments.Name,
                //DefaultType = Departments.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(Departments.DefaultType.ToString(), requestInfo.Lang),
                IsActive = Departments.IsActive,
            }).ToListAsync();

            return new GetDefaultDepartmentsResponseDTO
            {
                DefaultDepartments = DepartmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultDepartmentsDropDownResponseDTO> GetForDropDown(GetDefaultDepartmentsCriteria criteria)
        {
            criteria.IsActive = true;
            var DepartmentsRepository = repositoryManager.DefaultDepartmentsRepository;
            var query = DepartmentsRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = DepartmentsRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var DepartmentsList = await queryPaged.Select(e => new GetDefaultDepartmentsForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultDepartmentsDropDownResponseDTO
            {
                DefaultDepartments = DepartmentsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultDepartmentsInfoResponseDTO> GetInfo(int DepartmentsId)
        {
            var Departments = await repositoryManager.DefaultDepartmentsRepository.Get(e => e.LookupType == LookupsType.Departments && e.Id == DepartmentsId && !e.IsDeleted)
                .Select(Departments => new GetDefaultDepartmentsInfoResponseDTO
                {
                    Code = Departments.Code,
                    Name = Departments.Name,
                    //DefaultType = Departments.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(Departments.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = Departments.IsActive,
                    NameTranslations = Departments.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);

            return Departments;
        }
        public async Task<GetDefaultDepartmentsByIdResponseDTO> GetById(int DepartmentsId)
        {
            var Departments = await repositoryManager.DefaultDepartmentsRepository.Get(e => e.LookupType == LookupsType.Departments && e.Id == DepartmentsId && !e.IsDeleted)
                .Select(Departments => new GetDefaultDepartmentsByIdResponseDTO
                {
                    Id = Departments.Id,
                    Code = Departments.Code,
                    Name = Departments.Name,
                    //DefaultType = Departments.DefaultType,
                    IsActive = Departments.IsActive,
                    NameTranslations = Departments.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);

            return Departments;

        }
        public async Task<bool> Delete(int DepartmentsId)
        {
            var Departments = await repositoryManager.DefaultDepartmentsRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.Departments && d.Id == DepartmentsId) ??
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);
            Departments.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int DepartmentsId)
        {
            var Departments = await repositoryManager.DefaultDepartmentsRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.Departments && !d.IsActive && d.Id == DepartmentsId) ??
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);
            Departments.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultDepartmentsRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.Departments && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
