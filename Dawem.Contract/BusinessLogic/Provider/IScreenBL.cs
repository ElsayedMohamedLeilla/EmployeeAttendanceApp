using Dawem.Models.Dtos.Lookups;

namespace Dawem.Contract.BusinessLogic.Provider
{
    public interface IScreenBL
    {
        Task<int> Create(CreatedScreen screen);
        Task<List<ScreenDto>> GetAllDescendantScreens(int id);
        Task<bool> Delete(int id);
    }
}
