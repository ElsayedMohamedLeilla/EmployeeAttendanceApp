using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Dtos.Lookups;

namespace SmartBusinessERP.BusinessLogic.Provider.Contract
{
    public interface IScreenBL
    {
        Task<BaseResponseT<CreatedScreen>> Create(CreatedScreen screen);

        BaseResponseT<IEnumerable<ScreenDto>> GetAllDescendantScreens(int id);

        BaseResponseT<bool> DeleteScreen(int id);
    }
}
