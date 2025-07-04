﻿using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.JustificationsTypes;
using Dawem.Models.Response.Dawem.Core.JustificationsTypes;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface IJustificationTypeBL
    {
        Task<int> Create(CreateJustificationsTypeDTO model);
        Task<bool> Update(UpdateJustificationsTypeDTO model);
        Task<GetJustificationsTypeInfoResponseDTO> GetInfo(int justificationsTypeId);
        Task<GetJustificationsTypeByIdResponseDTO> GetById(int justificationsTypeId);
        Task<GetJustificationsTypeResponseDTO> Get(GetJustificationsTypesCriteria model);
        Task<GetJustificationsTypeDropDownResponseDTO> GetForDropDown(GetJustificationsTypesCriteria model);
        Task<bool> Delete(int justificationsTypeId);
        Task<GetJustificationsTypesInformationsResponseDTO> GetJustificationTypesInformations();
    }
}
