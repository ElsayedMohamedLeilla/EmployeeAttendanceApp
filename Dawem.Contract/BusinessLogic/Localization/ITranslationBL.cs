using Dawem.Models.Response;

namespace Dawem.Contract.BusinessLogic.Localization
{
    public interface ITranslationBL

    {

        BaseResponseT<bool> RefreshCachedTranslation();



    }
}