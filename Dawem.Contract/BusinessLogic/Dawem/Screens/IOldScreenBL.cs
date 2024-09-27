using Dawem.Models.Dtos.Dawem.Lookups;

namespace Dawem.Contract.BusinessLogic.Dawem.Screens
{
    public interface IOldScreenBL
    {
        Task<int> Create(CreatedScreen screen);
        Task<List<ScreenDto>> GetAllDescendantScreens(int id);
        Task<bool> Delete(int id);
    }
}
