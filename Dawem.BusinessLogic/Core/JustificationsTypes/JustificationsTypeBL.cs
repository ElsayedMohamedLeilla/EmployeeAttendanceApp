using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.JustificationsTypes;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.JustificationsTypes;
using Dawem.Translations;
using Dawem.Validation.FluentValidation.Core.JustificationsTypes;
using Dawem.Validation.FluentValidation.Employees;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Core.JustificationsTypes
{
    public class JustificationsTypeBL : IJustificationsTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IJustificationsTypeBLValidation justificationsTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly LinkGenerator generator;


        public JustificationsTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
         IUploadBLC _uploadBLC,
         LinkGenerator _generator,
         IWebHostEnvironment _webHostEnvironment,
        RequestInfo _requestHeaderContext,
        IJustificationsTypeBLValidation _justificationsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            justificationsTypeBLValidation = _justificationsTypeBLValidation;
            mapper = _mapper;
            generator = _generator;
            webHostEnvironment = _webHostEnvironment;
        }
        public async Task<int> Create(CreateJustificationsTypeDTO model)
        {
            #region Model Validation

            var createJustificationsTypeModel = new CreateJustificationsTypeModelValidator();
            var createJustificationsTypeModelResult = createJustificationsTypeModel.Validate(model);
            if (!createJustificationsTypeModelResult.IsValid)
            {
                var error = createJustificationsTypeModelResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await justificationsTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert JustificationsType

            #region Set JustificationsType code

            var getNextCode = await repositoryManager.JustificationsTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var justificationsType = mapper.Map<JustificationsType>(model);
            justificationsType.CompanyId = requestInfo.CompanyId;
            justificationsType.AddUserId = requestInfo.UserId;
            justificationsType.Code = getNextCode;
            repositoryManager.JustificationsTypeRepository.Insert(justificationsType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return justificationsType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateJustificationsTypeDTO model)
        {
            #region Model Validation

            var updateJustificationsTypeModelValidator = new UpdateJustificationsTypeModelValidator();
            var updateJustificationsTypeModelValidatorResult = updateJustificationsTypeModelValidator.Validate(model);
            if (!updateJustificationsTypeModelValidatorResult.IsValid)
            {
                var error = updateJustificationsTypeModelValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            #region Business Validation

            await justificationsTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();
            #region Update JustificationsType
            var getJustificationsType = await repositoryManager.JustificationsTypeRepository.GetByIdAsync(model.Id);
            getJustificationsType.Name = model.Name;
            getJustificationsType.IsActive = model.IsActive;
            getJustificationsType.ModifiedDate = DateTime.Now;
            getJustificationsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetJustificationsTypeResponseDTO> Get(GetJustificationsTypeCriteria criteria)
        {
            #region Model Validation

            var getValidator = new GetGenaricValidator(); // validate on pageining and all common validation 
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            var justificationsTypeRepository = repositoryManager.JustificationsTypeRepository;
            var query = justificationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = justificationsTypeRepository.OrderBy(query, nameof(JustificationsType.Id), DawemKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var justificationsTypesList = await queryPaged.Select(e => new GetJustificationsTypeResponseModelDTO
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();

            return new GetJustificationsTypeResponseDTO
            {
                JustificationsTypes = justificationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetJustificationsTypeDropDownResponseDTO> GetForDropDown(GetJustificationsTypeCriteria criteria)
        {
            #region Model Validation

            var getValidator = new GetGenaricValidator();
            var getValidatorResult = getValidator.Validate(criteria);
            if (!getValidatorResult.IsValid)
            {
                var error = getValidatorResult.Errors.FirstOrDefault();
                throw new BusinessValidationException(error.ErrorMessage);
            }

            #endregion

            criteria.IsActive = true;
            var JustificationsTypeRepository = repositoryManager.JustificationsTypeRepository;
            var query = JustificationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = JustificationsTypeRepository.OrderBy(query, nameof(JustificationsType.Id), DawemKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var justificationsTypesList = await queryPaged.Select(e => new GetJustificationsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetJustificationsTypeDropDownResponseDTO
            {
                JustificationsTypes = justificationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetJustificationsTypeInfoResponseDTO> GetInfo(int justificationsTypeId)
        {
            var justificationsType = await repositoryManager.JustificationsTypeRepository.Get(e => e.Id == justificationsTypeId && !e.IsDeleted)
                .Select(e => new GetJustificationsTypeInfoResponseDTO
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryJustificationsTypeNotFound);

            return justificationsType;
        }
        public async Task<GetJustificationsTypeByIdResponseDTO> GetById(int justificationsTypeId)
        {
            var justificationsType = await repositoryManager.JustificationsTypeRepository.Get(e => e.Id == justificationsTypeId && !e.IsDeleted)
                .Select(e => new GetJustificationsTypeByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(DawemKeys.SorryJustificationsTypeNotFound);

            return justificationsType;

        }
        public async Task<bool> Delete(int JustificationsTypeId)
        {
            var justificationsType = await repositoryManager.JustificationsTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == JustificationsTypeId) ??
                throw new BusinessValidationException(DawemKeys.SorryJustificationsTypeNotFound);


            repositoryManager.JustificationsTypeRepository.Delete(justificationsType);


            await unitOfWork.SaveAsync();
            return true;
        }

    }
}
