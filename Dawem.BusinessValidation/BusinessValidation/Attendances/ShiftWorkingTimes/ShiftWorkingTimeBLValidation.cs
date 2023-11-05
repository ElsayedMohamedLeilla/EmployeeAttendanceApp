using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.Core
{
    public class ShiftWorkingTimeBLValidation : IShiftWorkingTimeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public ShiftWorkingTimeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateShiftWorkingTimeModelDTO model)
        {
            var checkShiftWorkingTimeDuplicate = await repositoryManager
                .ShiftWorkingTimeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkShiftWorkingTimeDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryShiftWorkingTimeNameIsDuplicated);
            }

            return true;
        }


        public async Task<bool> UpdateValidation(UpdateShiftWorkingTimeModelDTO model)
        {
            var checkShiftWorkingTimeDuplicate = await repositoryManager
                .ShiftWorkingTimeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkShiftWorkingTimeDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryShiftWorkingTimeNameIsDuplicated);
            }

            return true;
        }


    }
}
