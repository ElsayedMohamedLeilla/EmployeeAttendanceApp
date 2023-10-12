using AutoMapper;
using SmartBusinessERP.BusinessLogic.Core.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Inventory;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.DtosMappers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Repository.Core.Conract;

namespace SmartBusinessERP.BusinessLogic.Core
{
    public class UnitBranchBL : IUnitBranchBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IUnitBranchRepository unitBranchRepository;
        private readonly IMapper mapper;
        public UnitBranchBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IUnitBranchRepository _unitBranchRepository, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            unitBranchRepository = _unitBranchRepository;
            mapper = _mapper;
        }


        public BaseResponseT<UnitBranchDTO> Get(int Id)
        {
            throw new NotImplementedException();
        }
        public BaseResponseT<List<UnitBranchDTO>> Get()
        {
            throw new NotImplementedException();
        }
        public BaseResponseT<UnitBranch> Create(UnitBranch unitBranch)
        {
            BaseResponseT<UnitBranch> response = new BaseResponseT<UnitBranch>();
            try
            {
                response.Result = unitBranchRepository.Insert(unitBranch);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }
        public BaseResponseT<List<UnitBranchDTO>> GetByUnit(int unitId)
        {
            BaseResponseT<List<UnitBranchDTO>> response = new BaseResponseT<List<UnitBranchDTO>>();
            try
            {
                var unitBranches = unitBranchRepository
                    .Get(user => user.UnitId == unitId).ToList();
                if (unitBranches != null && unitBranches.Count() > 0)
                {
                    response.Result = UnitBranchDTOMapper.Map(unitBranches);
                    response.Status = ResponseStatus.Success;
                }
                else
                {
                    response.Result = null;
                    response.Status = ResponseStatus.ValidationError;
                    TranslationHelper.SetValidationMessages(response, "NoBranchForThisUser", "No Branch For This User !");

                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }
        public BaseResponseT<bool> Update(UnitBranch unitBranch)
        {
            var response = new BaseResponseT<bool>();
            try
            {
                unitBranchRepository.Update(unitBranch);
                unitOfWork.Save();
                response.Result = true;
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Result = false;
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }
        public BaseResponseT<bool> Delete(int Id)
        {
            throw new NotImplementedException();
        }
        public BaseResponseT<List<UnitBranch>> BulkCreate(List<UnitBranch> unitBranches)
        {
            BaseResponseT<List<UnitBranch>> response = new BaseResponseT<List<UnitBranch>>();
            try
            {
                response.Result = unitBranchRepository.BulkInsert(unitBranches).ToList();
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }

    }
}

