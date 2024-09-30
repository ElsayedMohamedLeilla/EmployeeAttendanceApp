using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultDepartments;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultDepartmentsBLValidation : IDefaultDepartmentsBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultDepartmentsBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultDepartmentsDTO model)
        {
            var checkDepartmentsDuplicate = await repositoryManager
                .DefaultDepartmentsRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.Departments).AnyAsync();
            if (checkDepartmentsDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisDepartment);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultDepartmentsDTO model)
        {
            var checkDepartmentsDuplicate = await repositoryManager
                .DefaultDepartmentsRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.Departments).AnyAsync();
            if (checkDepartmentsDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryDepartmentNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisVacation);
            }

            return true;
        }


    }
}
