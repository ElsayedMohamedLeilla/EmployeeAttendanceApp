using Dawem.Models.Dtos.Lookups;
using Dawem.Models.Response;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IScreenBL
    {
        Task<BaseResponseT<CreatedScreen>> Create(CreatedScreen screen);

        BaseResponseT<IEnumerable<ScreenDto>> GetAllDescendantScreens(int id);

        BaseResponseT<bool> DeleteScreen(int id);
    }
}
