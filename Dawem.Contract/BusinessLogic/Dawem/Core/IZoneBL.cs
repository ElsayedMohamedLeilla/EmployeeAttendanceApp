﻿using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.Zones;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Response.Dawem.Core.Zones;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
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
        Task<GetZonesInformationsResponseDTO> GetZonesInformations();

        public Task<MemoryStream> ExportDraft();
        public Task<Dictionary<string, string>> ImportDataFromExcelToDB(Stream importedFile);

    }
}
