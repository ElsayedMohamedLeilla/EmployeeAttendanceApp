using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Screens;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Dtos.Dawem.Lookups;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Provider
{
    public class OldScreenBL : IOldScreenBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public OldScreenBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IMapper _mapper, IRepositoryManager _repositoryManager)
        {
            unitOfWork = _unitOfWork;
            repositoryManager = _repositoryManager;
            mapper = _mapper;
        }

        public async Task<bool> Delete(int Id)
        {
            repositoryManager.OldScreenRepository.Delete(Id);
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<int> Create(CreatedScreen screen)
        {
            unitOfWork.CreateTransaction();
            var parentScreen = await repositoryManager.OldScreenRepository.Get(p => p.Id == screen.ParentId).FirstOrDefaultAsync();
            if (parentScreen != null)
            {
                screen.Level = parentScreen.Level + 1;
            }
            else
            {
                screen.Level = 1;
            }
            var dbScreen = mapper.Map<OldNotUsedScreen>(screen);
            repositoryManager.OldScreenRepository.Insert(dbScreen);
            await unitOfWork.SaveAsync();

            await unitOfWork.CommitAsync();
            return dbScreen.Id;
        }


        public async Task<List<ScreenDto>> GetAllDescendantScreens(int id)
        {
            var screens = new List<OldNotUsedScreen>();
            var parentScreen = await repositoryManager.OldScreenRepository.Get(a => a.Id == id, IncludeProperties: "ScreenModules")
                .FirstOrDefaultAsync(s => s.Id == id);
            if (parentScreen == null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryScreenNotFound);
            }

            screens.Add(parentScreen);
            screens.AddRange(GetChildScreens(parentScreen));
            var result = mapper.Map<List<ScreenDto>>(screens);
            return result;
        }

        private void DeleteDescendantScreens(OldNotUsedScreen screen)
        {
            foreach (var childScreen in screen.Children)
            {
                repositoryManager.OldScreenRepository.Delete(childScreen);
                DeleteDescendantScreens(childScreen);
            }
        }


        private IEnumerable<OldNotUsedScreen> GetChildScreens(OldNotUsedScreen parentScreen)
        {
            var childScreens = parentScreen.Children;
            foreach (var childScreen in childScreens)
            {
                yield return childScreen;
                foreach (var grandChildScreen in GetChildScreens(childScreen))
                {
                    yield return grandChildScreen;
                }
            }
        }

    }
}
