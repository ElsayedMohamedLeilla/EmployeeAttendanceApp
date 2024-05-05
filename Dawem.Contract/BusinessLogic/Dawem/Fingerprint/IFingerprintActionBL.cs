using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Contract.BusinessLogic.Dawem.Attendances
{
    public interface IFingerprintActionBL
    {
        Task<bool> ReadFingerprint(ReadFingerprintModel model);
    }
}
