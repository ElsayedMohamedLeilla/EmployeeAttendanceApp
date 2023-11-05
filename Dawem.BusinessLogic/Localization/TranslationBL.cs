using Dawem.Contract.BusinessLogic.Localization;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Response;
using Dawem.Translations;

namespace Dawem.BusinessLogic.Localization
{
    public class TranslationBL : ITranslationBL
    {

        private readonly IRepositoryManager repositoryManager;

        private readonly IUnitOfWork<ApplicationDBContext> _unitOfWork;
        public TranslationBL(IUnitOfWork<ApplicationDBContext> unitOfWork,IRepositoryManager _repositoryManager)
        {
            _unitOfWork = unitOfWork;
            repositoryManager = _repositoryManager;
        }

        public BaseResponseT<bool> RefreshCachedTranslation()
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {
                var query = repositoryManager.TranslationRepository.Get();
                var translations = query.ToList();
                if (translations != null)
                {
                    var ar = translations.FindAll(c => c.Lang == LeillaKeys.Ar1);
                    if (ar.Count > 0)
                    {
                        TranslationHelper.SetArTrans(ar.Select(x => new TransModel { KeyWord = x.KeyWord, TransWords = x.TransWords }));
                    }

                    var en = translations.FindAll(c => c.Lang == LeillaKeys.En2);
                    if (en.Count > 0)
                    {
                        TranslationHelper.SetEnTrans(en.Select(x => new TransModel { KeyWord = x.KeyWord, TransWords = x.TransWords }));
                    }
                }
                response.Result = true;
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }


    }
}
