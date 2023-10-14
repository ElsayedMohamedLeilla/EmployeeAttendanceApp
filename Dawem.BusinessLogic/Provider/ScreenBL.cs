using Dawem.Contract.BusinessLogic.Provider;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Dtos.Lookups;

namespace Dawem.BusinessLogic.Provider
{
    public class ScreenBL : IScreenBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IRepositoryManager repositoryManager;
        public ScreenBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IRepositoryManager _repositoryManager)
        {
            unitOfWork = _unitOfWork;
            repositoryManager = _repositoryManager;
        }

        public async Task<bool> Delete(int Id)
        {
            repositoryManager.screenRepository.Delete(Id);
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<CreatedScreen> Create(CreatedScreen screen)
        {
                unitOfWork.CreateTransaction();
                var parentScreen = repositoryManager.screenRepository.Get(p => p.Id == screen.ParentId).FirstOrDefault();
                if (parentScreen != null)
                {
                    screen.Level = parentScreen.Level + 1;
                }
                else
                {
                    screen.Level = 1;
                }
                var dbScreen = mapper.Map<Screen>(screen);

                screenRepository.Insert(dbScreen);
                await unitOfWork.SaveAsync();

                unitOfWork.Commit();
                response.Status = ResponseStatus.Success;

           


            return response;
        }


        public BaseResponseT<IEnumerable<ScreenDto>> GetAllDescendantScreens(int id)
        {
            var response = new BaseResponseT<IEnumerable<ScreenDto>>
            {
                Status = ResponseStatus.Success
            };

            var result = new List<Screen>();
            var parentScreen = screenRepository.Get(a => a.Id == id, IncludeProperties: "ScreenModules")
                .FirstOrDefault(s => s.Id == id);

            if (parentScreen == null)
            {
                response.Result = null;
                response.Status = ResponseStatus.NotFound;
                return response;
            }

            result.Add(parentScreen);
            result.AddRange(GetChildScreens(parentScreen));
            var screenDto = mapper.Map<List<ScreenDto>>(result);
            response.Result = screenDto;
            return response;
        }

        public BaseResponseT<bool> DeleteScreen(int id)
        {
            var response = new BaseResponseT<bool>
            {
                Status = ResponseStatus.Success
            };

            var screen = screenRepository.Get(a => a.Id == id, IncludeProperties: "ScreenModules").FirstOrDefault(s => s.Id == id);

            if (screen == null)
            {
                TranslationHelper.SetResponseMessages(response, "NoScreenFound", "Screen Not Found");
                response.Result = true;
                response.Status = ResponseStatus.NotFound;
                return response;
            }
            try
            {
                unitOfWork.CreateTransaction();
                DeleteDescendantScreens(screen);

                screenRepository.Delete(screen);
                unitOfWork.Save();
                unitOfWork.Commit();
                return response;
            }
            catch (Exception ex)

            {

                TranslationHelper.SetResponseMessages(response, "DeleteScreenError", "You Can't Delete This Screen Because It Assign To Other Permissions");
            }
            return response;
        }

        private void DeleteDescendantScreens(Screen screen)
        {
            foreach (var childScreen in screen.ScreenModules)
            {
                screenRepository.Delete(childScreen);
                DeleteDescendantScreens(childScreen);
            }
        }


        private IEnumerable<Screen> GetChildScreens(Screen parentScreen)
        {
            var childScreens = parentScreen.ScreenModules;
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
