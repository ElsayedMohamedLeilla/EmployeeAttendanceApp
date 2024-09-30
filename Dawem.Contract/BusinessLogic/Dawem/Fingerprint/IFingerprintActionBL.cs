using Dawem.Models.Dtos.Dawem.Fingerprint;

namespace Dawem.Contract.BusinessLogic.Dawem.Attendances
{
    public interface IFingerprintActionBL
    {
        Task<bool> ReadFingerprint(ReadFingerprintModel model);
        Task<bool> AddFingerprintLog(ReadFingerprintModel model);
    }
}
