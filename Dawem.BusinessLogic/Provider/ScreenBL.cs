using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Data;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Repository.Provider.Contract;
using AutoMapper;
using SmartBusinessERP.Models.Dtos.Lookups;
using SmartBusinessERP.Repository.Lookups.Contract;
using SmartBusinessERP.Domain.Entities.Lookups;
using Dawem.Contract.BusinessLogic.Provider;

namespace Dawem.BusinessLogic.Provider
{
    public class ScreenBL : IScreenBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IPackageRepository packageRepository;
        private readonly IPackageScreenBL packagescreenOrch;
        private readonly IScreenRepository screenRepository;
        private readonly IBranchRepository branchRepository;
        private readonly IMapper mapper;



        public ScreenBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IPackageRepository _packageRepository,
            IPackageScreenBL _packagescreenOrch, IScreenRepository _screenRepository, IMapper _mapper, IBranchRepository _branchRepository)
        {
            unitOfWork = _unitOfWork;
            packageRepository = _packageRepository;
            packagescreenOrch = _packagescreenOrch;
            screenRepository = _screenRepository;
            branchRepository = _branchRepository;

            mapper = _mapper;

        }

        public BaseResponseT<bool> Delete(int Id)
        {
            var response = new BaseResponseT<bool>
            {
            };
            try
            {
                packageRepository.Delete(Id);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);

            }

            return response;
        }

        public async Task<BaseResponseT<CreatedScreen>> Create(CreatedScreen screen)
        {
            var response = new BaseResponseT<CreatedScreen>();

            try
            {
                unitOfWork.CreateTransaction();

                var parentScreen = screenRepository.Get(p => p.Id == screen.ParentId).FirstOrDefault();
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

            }

            catch (Exception ex)

            {
                TranslationHelper.SetException(response, ex);
            }


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
