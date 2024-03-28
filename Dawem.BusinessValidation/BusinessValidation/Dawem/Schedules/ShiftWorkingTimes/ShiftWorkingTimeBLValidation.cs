using Dawem.Contract.BusinessValidation.Dawem.Schedules.ShiftWorkingTimes;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Schedules.ShiftWorkingTimes;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Schedules.ShiftWorkingTimes
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
                throw new BusinessValidationException(LeillaKeys.SorryShiftWorkingTimeNameIsDuplicated);
            }
            var ChechInAndCheckOutIsExsists = await repositoryManager
               .ShiftWorkingTimeRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.CheckInTime == model.CheckInTime && c.CheckOutTime == model.CheckOutTime).AnyAsync();
            if (ChechInAndCheckOutIsExsists)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThereIsAnotherShiftUserTheTimeSpanPleaseUseItOrChangeTimeSpan);
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
                throw new BusinessValidationException(LeillaKeys.SorryShiftWorkingTimeNameIsDuplicated);
            }

            return true;
        }


    }
}
