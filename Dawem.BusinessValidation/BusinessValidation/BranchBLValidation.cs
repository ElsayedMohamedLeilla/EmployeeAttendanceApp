using Dawem.Contract.BusinessValidation;
using Dawem.Contract.Repository.Provider;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Provider;
using Dawem.Models.Dtos.Shared;
using Dawem.Models.Response;
using Dawem.Models.Response.Provider;

namespace Dawem.Validation.BusinessValidation
{

    public class BranchBLValidation : IBranchBLValidation
    {
        private readonly RequestHeaderContext userContext;
        private readonly IBranchRepository branchRepository;
        private readonly IUserBranchRepository userBranchRepository;

        public BranchBLValidation(RequestHeaderContext _userContext, IBranchRepository _branchRepository, IUserBranchRepository _userBranchRepository)
        {

            userContext = _userContext;
            branchRepository = _branchRepository;
            userBranchRepository = _userBranchRepository;
        }
        public Task<BranchValidatorResult> BranchCreationValidator(BranchValidatorModel model)
        {
            BranchValidatorResult response = new();

            var Branch = model.Branch;

            try
            {
                response.Status = ResponseStatus.Success;
                response.Result = true;


                if (Branch.CountryId <= 0)
                {
                    response.Status = ResponseStatus.ValidationError;
                    response.Result = false;

                    TranslationHelper.SetSearchResultMessages(
                        response,
                        "YouMustChooseCountry!",
                        "You Must Choose Country Related To Branch !", lang: userContext.Lang);
                    return Task.FromResult(response);
                }



                response.Branch = Branch;

            }
            catch (Exception ex)
            {
                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return Task.FromResult(response);
        }

        public BaseResponseT<bool> ValidateChangeForMainBranchOnly(RequestHeaderContext userContext, ChangeType changeType)
        {
            BaseResponseT<bool> response = new();
            try
            {
                response.Status = ResponseStatus.Success;
                response.Result = true;



                if (userContext == null)
                {
                    response.Status = ResponseStatus.ValidationError;
                    response.Result = false;

                    TranslationHelper.SetResponseMessages(
                        response,
                        "SorryuserContextNull!", "Sorry UserContext Null !", lang: userContext.Lang);
                    return response;
                }



                if (!userContext.IsMainBranch && changeType == ChangeType.View)
                {
                    response.Status = ResponseStatus.ValidationError;
                    response.Result = false;

                    TranslationHelper.SetResponseMessages(response, "NotMainBranch", "Sorry!! You current branch is not main branch ..  ", lang: userContext.Lang);
                    return response;
                }

                if (!userContext.IsMainBranch && changeType == ChangeType.Add)
                {
                    response.Status = ResponseStatus.ValidationError;
                    response.Result = false;

                    TranslationHelper.SetResponseMessages(
                        response,
                        "SorryAddCanBeDoneInMainBranchOnly!", "Sorry Add Can Be Done In Main Branch Only !", lang: userContext.Lang);
                    return response;
                }

                if (!userContext.IsMainBranch && changeType == ChangeType.Edit)
                {
                    response.Status = ResponseStatus.ValidationError;
                    response.Result = false;

                    TranslationHelper.SetResponseMessages(
                        response,
                        "SorryEditCanBeDoneInMainBranchOnly!", "Sorry Edit Can Be Done In Main Branch Only !", lang: userContext.Lang);
                    return response;
                }

                if (!userContext.IsMainBranch && changeType == ChangeType.Delete)
                {
                    response.Status = ResponseStatus.ValidationError;
                    response.Result = false;

                    TranslationHelper.SetResponseMessages(
                        response,
                        "SorryDeleteCanBeDoneInMainBranchOnly!", "Sorry Delete Can Be Done In Main Branch Only !", lang: userContext.Lang);
                    return response;
                }

            }
            catch (Exception ex)
            {

                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }

        public async Task<validateUserBranchResult> ValidateUserBranch(ValidateUserBranchSearchCriteria criteria)
        {
            var response = new validateUserBranchResult();
            response.Status = ResponseStatus.Success;

            try
            {
                var BranchId = criteria.BranchId;


                if (BranchId <= 0)
                {
                    var uProviderQuery = userBranchRepository
                        .GetByCondition(ub => ub.UserId == criteria.UserId &&
                         ub.Branch.IsMainBranch == true)
                        .Select(ua => ua.BranchId);

                    var branchs = uProviderQuery.ToList();

                    if (branchs.Count > 1)
                    {
                        TranslationHelper.SetValidationMessages(response,
                            "SorryThereIsMoreThanOneBranchForChoosenUser." +
                            "YouMustChooseBranchToEnterWith!",
                            "Sorry There Is More Than One Branch For Choosen User. " +
                            "You Must Choose Branch To Enter With !", "",
                            userContext.Lang);
                        response.Status = ResponseStatus.ValidationError;
                        return response;
                    }


                    BranchId = branchs.FirstOrDefault();

                }





                var getUserBranch = userBranchRepository
                    .GetEntityByCondition(uas => uas.UserId == criteria.UserId && uas.BranchId == BranchId);

                if (getUserBranch == null)
                {
                    TranslationHelper.SetValidationMessages(response,
                                            "Sorry!!ThisUserDoNotHaveAccessToSelectedBranch!",
                                            "Sorry !! This User Do Not Have Access To Selected Branch !", "",
                                            userContext.Lang);
                    response.Status = ResponseStatus.ValidationError;

                    return response;
                }




                response.BranchId = BranchId;

            }
            catch (Exception ex)
            {
                response.Exception = ex; response.Message = ex.Message;
                response.Status = ResponseStatus.Error;
            }

            return response;
        }




    }
}
