﻿using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.VacationsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Core
{
    public class VacationsTypeBLValidation : IVacationTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public VacationsTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateVacationsTypeDTO model)
        {
            var checkVacationsTypeDuplicate = await repositoryManager
                .VacationsTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkVacationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryVacationsTypeNameIsDuplicated);
            }

            return true;
        }


        public async Task<bool> UpdateValidation(UpdateVacationsTypeDTO model)
        {
            var checkVacationsTypeDuplicate = await repositoryManager
                .VacationsTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkVacationsTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryVacationsTypeNameIsDuplicated);
            }

            return true;
        }


    }
}
