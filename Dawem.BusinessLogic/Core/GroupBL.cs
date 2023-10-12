using AutoMapper;
using SmartBusinessERP.BusinessLogic.Core.Contract;
using SmartBusinessERP.BusinessLogic.Validators.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Core;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.DtosMappers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Repository.Core.Conract;

namespace SmartBusinessERP.BusinessLogic.Core
{
    public class GroupBL : IGroupBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IGroupRepository groupRepository;
        private readonly IBranchValidatorBL branchValidatorBL;
        private readonly IMapper mapper;
        private readonly RequestHeaderContext userContext;


        public GroupBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IGroupRepository _groupRepository, RequestHeaderContext _userContext,
            IBranchValidatorBL _branchValidatorBL, IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            branchValidatorBL = _branchValidatorBL;
            groupRepository = _groupRepository;
            mapper = _mapper;
            userContext = _userContext;
        }

        public async Task<BaseResponseT<GroupDTO>> GetById(int Id)
        {
            BaseResponseT<GroupDTO> response = new();
            try
            {
                var result = groupRepository.Get(x => x.Id == Id).FirstOrDefault();
                if (result == null)
                {
                    TranslationHelper.SetResponseMessages(response, "NoDataFound", "No Data Found", "");
                    response.Status = ResponseStatus.Error;
                }
                else
                {
                    response.Result = GroupDTOMapper.Map(result);
                    response.Status = ResponseStatus.Success;
                }               
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }

        public async Task<GetGroupsResponse> Get(GetGroupsCriteria criteria)
        {
            var result = new GetGroupsResponse();

            try
            {
                var query = groupRepository.GetAsQueryable(criteria);
                var queryOrdered = groupRepository.OrderBy(query, "Id", "desc");

                #region paging

                var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                var take = PagingHelper.Take(criteria.PageSize);

                var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                var groups = queryPaged.ToList();
                GroupDTOMapper.InitGroupContext(userContext);
                result.Groups = GroupDTOMapper.Map(groups);
                result.Status = ResponseStatus.Success;
                result.TotalCount = query.Count();
            }
            catch (Exception ex)
            {
                result.Exception = ex; result.Message = ex.Message;
                result.Status = ResponseStatus.Error;
            }
            return result;
        }

        public async Task<GetGroupInfoResponse> GetInfo(GetGroupInfoCriteria criteria)
        {

            GetGroupInfoResponse groupSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                var group = await groupRepository.GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id);

                if (group != null)
                {
                    GroupDTOMapper.InitGroupContext(userContext);
                    var groupInfo = GroupDTOMapper.MapInfo(group);
                    groupSearchResult.GroupInfo = groupInfo;
                    groupSearchResult.Status = ResponseStatus.Success;
                }
                else
                {
                    groupSearchResult.Status = ResponseStatus.ValidationError;
                    TranslationHelper
                    .SetResponseMessages
                        (groupSearchResult, "GroupNotFound!",
                        "Group Not Found !", lang: userContext.Lang);
                }
            }
            catch (Exception ex)
            {
                groupSearchResult.Exception = ex;
                groupSearchResult.Status = ResponseStatus.Error;
            }
            return groupSearchResult;

        }

        public async Task<BaseResponseT<Group>> Create(Group group)
        {
            var response = new BaseResponseT<Group>();
            try
            {
                unitOfWork.CreateTransaction();

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Add);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }


                var ValidateGroupResult = ValidateGroup(group);

                if (ValidateGroupResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateGroupResult, destination: response);
                    return response;
                }
                group.MainBranchId = userContext.BranchId ?? 0;
                response.Result = groupRepository.Insert(group);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }

        public BaseResponseT<bool> ValidateGroup(Group group)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();

            try
            {
                response.Status = ResponseStatus.Success;



                //var getGroup = groupRepository
                //    .Get(pm => pm.MainBranchId == group.MainBranchId && pm.GroupType == GroupType.MainGroup && pm.GroupType ==
                //    group.GroupType && pm.Id != group.Id).ToList();

                //if (getGroup != null && getGroup.Count() > 0)
                //{

                //    response.Status = ResponseStatus.ValidationError;


                //    TranslationHelper.SetResponseMessages(response,
                //        "GroupTypeCanNotRepeated!",
                //        "Payment Method Type Can Not Repeated !", lang: userContext.Lang);

                //    return response;
                //}


                return response;
            }
            catch (Exception ex)
            {

                TranslationHelper.SetException(response, ex);
                return response;
            }
        }

        public async Task<BaseResponseT<bool>> Update(Group group)
        {
            var response = new BaseResponseT<bool>();
            try
            {
                unitOfWork.CreateTransaction();

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Edit);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    TranslationHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }


                var ValidateGroupResult = ValidateGroup(group);

                if (ValidateGroupResult.Status != ResponseStatus.Success)
                {
                    TranslationHelper.MapBaseResponse(source: ValidateGroupResult, destination: response);
                    unitOfWork.Rollback();
                    return response;
                }

                group.MainBranchId = userContext.BranchId ?? 0;
                groupRepository.Update(group);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                response.Result = false;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }

            response.Status = ResponseStatus.Success;
            return response;

        }

        public async Task<BaseResponseT<bool>> Delete(int Id)
        {
            BaseResponseT<bool> response = new BaseResponseT<bool>();
            try
            {

                var ValidateChangeForMainBranchOnlyResult = branchValidatorBL.ValidateChangeForMainBranchOnly(userContext, ChangeType.Delete);

                if (ValidateChangeForMainBranchOnlyResult.Status != ResponseStatus.Success)
                {
                    ResponseHelper.MapBaseResponse(source: ValidateChangeForMainBranchOnlyResult, destination: response);
                    return response;
                }


                await unitOfWork.SaveAsync();
                groupRepository.Delete(Id);
                await unitOfWork.SaveAsync();
                response.Result = true;
                response.Status = ResponseStatus.Success;
            }

            catch (Exception)
            {
                response.Status = ResponseStatus.ValidationError;
                response.Result = false;
                TranslationHelper.SetResponseMessages(response, "Can'tBeDeletedItIsRelatedToOtherData", "Can't Be Deleted It Is Related To Other Data !");

            }

            return response;
        }
    }

}

