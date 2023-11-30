using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Zones;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Response.Core.Zones;

namespace Dawem.Contract.BusinessLogic.Core
{
    public interface IZoneBL
    {
        Task<int> Create(CreateZoneDTO model);
        Task<bool> Update(UpdateZoneDTO model);
        Task<GetZoneInfoResponseDTO> GetInfo(int ZoneId);
        Task<GetZoneByIdResponseDTO> GetById(int ZoneId);
        Task<GetZoneResponseDTO> Get(GetZoneCriteria model);
        Task<GetZoneDropDownResponseDTO> GetForDropDown(GetZoneCriteria model);
        Task<bool> Enable(int ZoneId);
        Task<bool> Disable(DisableModelDTO model);
        Task<bool> Delete(int ZoneId);
    }
}
