using Dawem.Contract.BusinessValidation.Dawem.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.HolidayTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Employees
{

    public class HolidayTypeBLValidation : IHolidayTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public HolidayTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateHolidayTypeModel model)
        {
            var checkHolidayTypeDuplicate = await repositoryManager
                .HolidayTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkHolidayTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryHolidayTypeNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateHolidayTypeModel model)
        {
            var checkHolidayTypeDuplicate = await repositoryManager
                .HolidayTypeRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkHolidayTypeDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryHolidayTypeNameIsDuplicated);
            }

            return true;
        }
    }
}
