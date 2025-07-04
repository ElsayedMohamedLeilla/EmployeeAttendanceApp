﻿using Dawem.Models.Requests.Justifications;

namespace Dawem.Contract.BusinessValidation.Dawem.Requests
{
    public interface IRequestJustificationBLValidation
    {
        Task<int?> CreateValidation(CreateRequestJustificationDTO model);
        Task<int?> UpdateValidation(UpdateRequestJustificationDTO model);
    }
}
