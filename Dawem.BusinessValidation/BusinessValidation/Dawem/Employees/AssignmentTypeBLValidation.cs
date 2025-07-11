﻿using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.AssignmentTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Employees
{

    public class AssignmentTypeBLValidation : IAssignmentTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public AssignmentTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateAssignmentTypeModel model)
        {
            var checkAssignmentTypeDuplicate = await repositoryManager
                .AssignmentTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkAssignmentTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryAssignmentTypeNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateAssignmentTypeModel model)
        {
            var checkAssignmentTypeDuplicate = await repositoryManager
                .AssignmentTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkAssignmentTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryAssignmentTypeNameIsDuplicated);
            }

            return true;
        }
    }
}
