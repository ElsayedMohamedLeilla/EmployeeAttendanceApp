using Dawem.Contract.BusinessValidation.Dawem.Others;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Others.VacationBalances;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.VacationBalances
{

    public class VacationBalanceBLValidation : IVacationBalanceBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public VacationBalanceBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateVacationBalanceModel model)
        {
            var checkVacationBalanceDuplicate = await repositoryManager
                .VacationBalanceRepository.Get(c => model.EmployeeId != null && !c.IsDeleted &&
                c.CompanyId == requestInfo.CompanyId
                && c.EmployeeId == model.EmployeeId
                && c.Year == model.Year
                && c.DefaultVacationType == model.DefaultVacationType).AnyAsync();

            if (checkVacationBalanceDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryVacationBalanceIsDuplicatedWithSameEmployeeAndYearAndVacationType);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateVacationBalanceModel model)
        {
            var checkVacationBalanceDuplicate = await repositoryManager
               .VacationBalanceRepository.Get(c => !c.IsDeleted &&
                c.Id != model.Id &&
               c.CompanyId == requestInfo.CompanyId
               && c.EmployeeId == model.EmployeeId
               && c.Year == model.Year
               && c.DefaultVacationType == model.DefaultVacationType).AnyAsync();

            if (checkVacationBalanceDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryVacationBalanceIsDuplicatedWithSameEmployeeAndYearAndVacationType);
            }

            return true;
        }
    }
}
