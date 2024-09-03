using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.AdminPanel.DefaultLookups
{
    public class DefaultTasksTypeBLValidation : IDefaultTaskTypeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo; // header
        public DefaultTasksTypeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }

        public async Task<bool> CreateValidation(CreateDefaultTasksTypeDTO model)
        {
            var checkTasksTypeDuplicate = await repositoryManager
                .DefaultTaskTypeRepository.Get(c => c.Name == model.Name && c.LookupType == LookupsType.TasksTypes).AnyAsync();
            if (checkTasksTypeDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryTasksTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisTask);
            }


            return true;
        }


        public async Task<bool> UpdateValidation(UpdateDefaultTasksTypeDTO model)
        {
            var checkTasksTypeDuplicate = await repositoryManager
                .DefaultTaskTypeRepository.Get(c =>
                c.Name == model.Name && c.Id != model.Id && c.LookupType == LookupsType.TasksTypes).AnyAsync();
            if (checkTasksTypeDuplicate)
            {
                throw new BusinessValidationException(AmgadKeys.SorryTasksTypeNameIsDuplicated);
            }
            if (model.NameTranslations.Count < 2) // user must enter arabic and english values as minimun
            {
                throw new BusinessValidationException(AmgadKeys.SorryYouMustEnterNameArabicAndEnglishAtLeastForThisTask);
            }

            return true;
        }


    }
}
