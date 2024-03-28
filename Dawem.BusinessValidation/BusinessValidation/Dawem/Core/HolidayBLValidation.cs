using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.Holidays;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Core
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
                && c.StartDate.Date == model.StartDate.Date && c.EndDate.Date == model.EndDate.Date).AnyAsync();
            if (checkHolidayDuplicateDate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThereisAnotherHolidayUseThisPeriod);
            }

            if (model.StartDate.Day < 1 || model.StartDate.Day < 1)
            {
                throw new BusinessValidationException(AmgadKeys.SorryDayCantBeLessThanOne);

            }
            // Handle Feb Month
            if (model.StartDate.Month == 2 && model.StartDate.Day > 29 || model.EndDate.Month == 2 && model.EndDate.Day > 29)
            {
                throw new BusinessValidationException(AmgadKeys.SorryFebrauryMonthCantBeMoreThan29Day);
            }
            // months 31 Day
            if ((model.StartDate.Month == 1 || model.StartDate.Month == 3 || model.StartDate.Month == 5 || model.StartDate.Month == 7 || model.StartDate.Month == 8 || model.StartDate.Month == 10 || model.StartDate.Month == 12) && model.StartDate.Day > 31 ||
                (model.EndDate.Month == 1 || model.EndDate.Month == 3 || model.EndDate.Month == 5 || model.EndDate.Month == 7 || model.EndDate.Month == 8) && model.StartDate.Day > 31)
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            // months 30 Day
            if ((model.StartDate.Month == 4 || model.StartDate.Month == 6 || model.StartDate.Month == 9 || model.StartDate.Month == 11) && model.StartDate.Day > 30 ||
                (model.EndDate.Month == 1 || model.EndDate.Month == 3 || model.EndDate.Month == 5 || model.EndDate.Month == 7 || model.EndDate.Month == 8) && model.EndDate.Day > 31)
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            if (model.StartDate.Month > 12 || model.EndDate.Month > 12 || model.StartDate.Month < 1 || model.StartDate.Month < 1)
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
                && c.StartDate.Day == model.StartDate.Day && c.EndDate.Day == model.EndDate.Day
                && c.StartDate.Month == model.StartDate.Month && c.EndDate.Month == model.EndDate.Month && c.Id != model.Id && c.DateType != model.DateType).AnyAsync();
            if (checkHolidayDuplicateDate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThereisAnotherHolidayUseThisPeriod);
            }

            if (model.StartDate.Day < 1 || model.StartDate.Day < 1)
            {
                throw new BusinessValidationException(AmgadKeys.SorryDayCantBeLessThanOne);
            }
            // Handle Feb Month
            if (model.StartDate.Month == 2 && model.StartDate.Month > 29 || model.EndDate.Month == 2 && model.StartDate.Day > 29)
            {
                throw new BusinessValidationException(AmgadKeys.SorryFebrauryMonthCantBeMoreThan29Day);
            }
            // months 31 Day
            if ((model.StartDate.Month == 1 || model.StartDate.Month == 3 || model.StartDate.Month == 5 || model.StartDate.Month == 7 || model.StartDate.Month == 8 || model.StartDate.Month == 10 || model.StartDate.Month == 12) && model.StartDate.Day > 31 ||
                (model.EndDate.Month == 1 || model.EndDate.Month == 3 || model.EndDate.Month == 5 || model.EndDate.Month == 7 || model.EndDate.Month == 8) && model.EndDate.Day > 31)
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            // months 30 Day
            if ((model.StartDate.Month == 4 || model.StartDate.Month == 6 || model.StartDate.Month == 9 || model.StartDate.Month == 11) && model.StartDate.Day > 30 ||
                (model.EndDate.Month == 1 || model.EndDate.Month == 3 || model.EndDate.Month == 5 || model.EndDate.Month == 7 || model.EndDate.Month == 8) && model.EndDate.Day > 31)
            {
                throw new BusinessValidationException(AmgadKeys.SorryPleaseEnterAvalidDate);
            }
            if (model.StartDate.Month > 12 || model.EndDate.Month > 12 || model.StartDate.Month < 1 || model.EndDate.Month < 1)
            {
                throw new BusinessValidationException(AmgadKeys.SorryMonthCantLessThanOneAndGreaterThanTwelve);

            }

            return true;
        }


    }
}
