using SmartBusinessERP.Domain.Entities.Packages;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Data;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Repository.Provider.Contract;
using SmartBusinessERP.BusinessLogic.Provider.Contract;
using SmartBusinessERP.Models.Criteria.Provider;
using SmartBusinessERP.Models.Dtos.Provider;
using AutoMapper;
using SmartBusinessERP.Models.Response.Provider;

namespace SmartBusinessERP.BusinessLogic.Provider
{
    public class PackageBL : IPackageBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IPackageRepository packageRepository;
        private readonly IPackageScreenBL packagescreenOrch;
        private readonly IPackageScreenRepository packageScreenRepository;
        private readonly IBranchRepository branchRepository;
        private readonly IMapper mapper;

       

        public PackageBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IPackageRepository _packageRepository,
            IPackageScreenBL _packagescreenOrch, IPackageScreenRepository _packageScreenRepository, IMapper _mapper, IBranchRepository _branchRepository)
        {
            unitOfWork = _unitOfWork;
            packageRepository = _packageRepository;
            packagescreenOrch = _packagescreenOrch;
            packageScreenRepository = _packageScreenRepository;
            branchRepository = _branchRepository;
          
                  mapper = _mapper;

        }
    
        public BaseResponseT<bool> Delete(int Id)
        {
            var response = new BaseResponseT<bool>
            {
            };
            try
            {
                packageRepository.Delete(Id);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);

            }

            return response;
        }

        public PackageSearchResult Get(PackageCriteria criteria)
        {
            var result = new PackageSearchResult
            {

            };
            try
            {

                string includeProperties = "PackageScreen";

                var query = packageRepository.GetAsQueryable(criteria, includeProperties: includeProperties);

                #region paging

                var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                var take = PagingHelper.Take(criteria.PageSize);
                var orderby = packageRepository.OrderBy(query, "Id", "desc");

                var queryPaged = criteria.PagingEnabled ? query.Skip(skip).Take(take) : query;
                #endregion

                result.Packages = mapper.Map<List<PackageDto>>(queryPaged.ToList());

                result.Status = ResponseStatus.Success;

                result.TotalCount = queryPaged.Count();

            }
            catch (Exception ex)
            {
                result.Exception = ex; result.Message = ex.Message;
                result.Status = ResponseStatus.Success;

            }

            return result;
        }

        public BaseResponseT<PackageDto> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public BaseResponseT<bool> Update(Package package)
        {
            var response = new BaseResponseT<bool>
            {
            };
            try
            {
                packageRepository.Update(package);
                unitOfWork.Save();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                TranslationHelper.SetException(response, ex);

            }
            return response;
        }

        public BaseResponseT<Package> Create(PackageDto packageDTO)
        {
            var response = new BaseResponseT<Package>();

            try
            {
                unitOfWork.CreateTransaction();

                var package = mapper.Map<Package>(packageDTO);

                packageRepository.Insert(package);
                unitOfWork.Save();
                
                unitOfWork.Commit();
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
