using Dawem.Models.Response;

namespace Dawem.Contract.BusinessLogic.Dawem.Localization
{
    public interface ITranslationBL

    {

        BaseResponseT<bool> RefreshCachedTranslation();



    }
}