using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Core.Holidaies;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.Core
{
    public class HolidayBLValidation : IHolidayBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public HolidayBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateHolidayDTO model)
        {
            var checkHolidayDuplicate = await repositoryManager
                .HolidayRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkHolidayDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNameIsDuplicated);
            }
            // Dublicated Date
            var checkHolidayDuplicateDate = await repositoryManager
                .HolidayRepository.Get(c => c.CompanyId == requestInfo.CompanyId
                && c.StartDay == model.StartDay && c.EndDay == model.EndDay
                && c.StartMonth == model.StartMonth && c.EndMonth == model.EndMonth).AnyAsync();
            if (checkHolidayDuplicateDate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThereisAnotherHolidayUseThisPeriod);
            }

            if (model.StartDay < 1 || model.EndDay < 1)
            {
                throw new BusinessValidationException(AmgadKeys.SorryDayCantBeLessThanOne);

            }
            // Handle Feb Month
            if ((model.StartMonth == 2 && model.StartDay > 29) || (model.EndMonth == 2 && model.EndDay > 29))
            {
                throw new BusinessValidationException(AmgadKeys.SorryFebrauryMonthCantBeMoreThan29Day);
            }
            // months 31 Day
            if (((model.StartMonth == 1 || model.StartMonth == 3 || model.StartMonth == 5 || model.StartMonth == 7 || model.StartMonth == 8 || model.StartMonth == 10 || model.StartMonth == 12) && model.StartDay > 31) ||
                ((model.EndMonth == 1 || model.EndMonth == 3 || model.EndMonth == 5 || model.EndMonth == 7 || model.EndMonth == 8) && model.EndDay > 31))
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            // months 30 Day
            if (((model.StartMonth == 4 || model.StartMonth == 6 || model.StartMonth == 9 || model.StartMonth == 11) && model.StartDay > 30) ||
                ((model.EndMonth == 1 || model.EndMonth == 3 || model.EndMonth == 5 || model.EndMonth == 7 || model.EndMonth == 8) && model.EndDay > 31))
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            if (model.StartMonth > 12 || model.EndMonth > 12 || model.StartMonth < 1 || model.EndMonth < 1)
            {
                throw new BusinessValidationException(AmgadKeys.SorryMonthCantLessThanOneAndGreaterThanTwelve);

            }

            return true;
        }


        public async Task<bool> UpdateValidation(UpdateHolidayDTO model)
        {
            var checkHolidayDuplicate = await repositoryManager
                .HolidayRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkHolidayDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNameIsDuplicated);
            }

            // Dublicated Date
            var checkHolidayDuplicateDate = await repositoryManager
                .HolidayRepository.Get(c => c.CompanyId == requestInfo.CompanyId
                && c.StartDay == model.StartDay && c.EndDay == model.EndDay
                && c.StartMonth == model.StartMonth && c.EndMonth == model.EndMonth).AnyAsync();
            if (checkHolidayDuplicateDate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThereisAnotherHolidayUseThisPeriod);
            }

            if (model.StartDay < 1 || model.EndDay < 1)
            {
                throw new BusinessValidationException(AmgadKeys.SorryDayCantBeLessThanOne);

            }
            // Handle Feb Month
            if ((model.StartMonth == 2 && model.StartDay > 29) || (model.EndMonth == 2 && model.EndDay > 29))
            {
                throw new BusinessValidationException(AmgadKeys.SorryFebrauryMonthCantBeMoreThan29Day);
            }
            // months 31 Day
            if (((model.StartMonth == 1 || model.StartMonth == 3 || model.StartMonth == 5 || model.StartMonth == 7 || model.StartMonth == 8 || model.StartMonth == 10 || model.StartMonth == 12) && model.StartDay > 31) ||
                ((model.EndMonth == 1 || model.EndMonth == 3 || model.EndMonth == 5 || model.EndMonth == 7 || model.EndMonth == 8) && model.EndDay > 31))
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            // months 30 Day
            if (((model.StartMonth == 4 || model.StartMonth == 6 || model.StartMonth == 9 || model.StartMonth == 11) && model.StartDay > 30) ||
                ((model.EndMonth == 1 || model.EndMonth == 3 || model.EndMonth == 5 || model.EndMonth == 7 || model.EndMonth == 8) && model.EndDay > 31))
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            if (model.StartMonth > 12 || model.EndMonth > 12 || model.StartMonth < 1 || model.EndMonth < 1)
            {
                throw new BusinessValidationException(AmgadKeys.SorryMonthCantLessThanOneAndGreaterThanTwelve);

            }

            return true;
        }


    }
}
