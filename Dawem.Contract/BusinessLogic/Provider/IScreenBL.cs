using Dawem.Models.Dtos.Lookups;
using Dawem.Models.Response;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IScreenBL
    {
        Task<BaseResponseT<CreatedScreen>> Create(CreatedScreen screen);

        BaseResponseT<IEnumerable<ScreenDto>> GetAllDescendantScreens(int id);

        BaseResponseT<bool> DeleteScreen(int id);
    }
}
