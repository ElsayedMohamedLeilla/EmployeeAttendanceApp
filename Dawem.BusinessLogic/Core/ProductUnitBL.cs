using Helpers;
using LinqKit;
using SmartBusinessERP.BusinessLogic.Core.Contract;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Data;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Dtos.Core;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Repository.Core.Conract;
using SmartBusinessERP.Models.Response;
using AutoMapper;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Domain.Entities.Inventory;

namespace SmartBusinessERP.BusinessLogic.Core
{
    public class ProductUnitBL : IProductUnitBL
    {
        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IProductUnitRepository productUnitRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IMapper mapper;
        private readonly RequestHeaderContext userContext;
        public ProductUnitBL(IUnitOfWork<ApplicationDBContext> _unitOfWork, IProductUnitRepository _productUnitRepository, IUnitRepository _unitRepository, IMapper _mapper, RequestHeaderContext _userContext)
        {
            unitOfWork = _unitOfWork;
            productUnitRepository = _productUnitRepository;
            unitRepository = _unitRepository;
            userContext = _userContext;
            mapper = _mapper;
        }
      

      
        public BaseResponseT<ProductUnitDto> GetById(int Id)
        {
            throw new NotImplementedException();
        }
        public ProductUnitSearchResult Get(ProductUnitSearchCriteria criteria)
        {

            var productUnitSearchResult = new ProductUnitSearchResult();

            try
            {
                var outerpredicate = PredicateBuilder.New<ItemUnit>(true);

                outerpredicate = outerpredicate.Start(x => x.BranchId == userContext.BranchId);

                var inner = PredicateBuilder.New<ItemUnit>(true);
                    if (!string.IsNullOrWhiteSpace(criteria.FreeText))
                    {
                        criteria.FreeText = criteria.FreeText.ToLower().Trim();

                    }

                    if (criteria.Id != null)
                    {
                        outerpredicate = outerpredicate.And(x => x.Id == criteria.Id);
                    }
                   

                    if (criteria.ProductId != null)
                    {
                        outerpredicate = outerpredicate.And(x => x.ItemId == criteria.ProductId);
                    }

                    if (criteria.UnitId != null)
                    {
                        outerpredicate = outerpredicate.And(x => x.UnitId == criteria.UnitId);
                    }
                    
                

                #region sorting

                #endregion

                #region paging

                var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                var take = PagingHelper.Take(criteria.PageSize);
                var orderby = SortingHelper<ItemUnit>.GetOrderBy("Id", "desc");

                var query = productUnitRepository.Get(outerpredicate, orderby).ToList();
                var queryPaged = criteria.PagingEnabled ? query.Skip(skip).Take(take) : query;

                #endregion

                var units = unitRepository.Get();
                var queryWithUnitName = from p in queryPaged
                                        join u in units on p.UnitId equals u.Id
                                        select new ProductUnitDto
                                        {
                                            BranchId = p.BranchId,
                                            Id = p.Id,
                                         
                                            IsMainUnit = p.IsMainUnit,
                                            //ProductId = p.ProductId,

                                            ProductBuyPrice = p.ProductBuyPrice,
                                            ProductSalePrice = p.ProductSalePrice,
                                            //RowState=p.RowState,
                                            UnitId = p.UnitId,
                                            UnitRate = p.UnitRate
                                        };


                var productUnits = queryWithUnitName.ToList();

                productUnitSearchResult.ProductUnits = productUnits;
                productUnitSearchResult.TotalCount = query.Count();
                productUnitSearchResult.Status = ResponseStatus.Success;

            }

            catch (Exception ex)
            {
                productUnitSearchResult.Status = ResponseStatus.Error;
                productUnitSearchResult.Exception = ex;
            }

            return productUnitSearchResult;
        }
        public BaseResponseT<float> GetUnitRate(int unitId, int productId)
        {
            BaseResponseT<float> response = new BaseResponseT<float>();
            try
            {
                response.Result = productUnitRepository.Get(c => c.UnitId == unitId && c.ItemId == productId)
                    .Select(c => c.UnitRate).FirstOrDefault();
                response.Status = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Result = -1;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
                
            }
            return response;
        }
        public BaseResponseT<ItemUnit> Create(ItemUnit productUnit)
        {
            throw new NotImplementedException();
        }
        public BaseResponseT<bool> Update(ItemUnit productUnit)
        {
            throw new NotImplementedException();
        }
        public BaseResponseT<bool> Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public BaseResponseT<List<ProductUnitDto>> GetByProduct(int productId)
        {
            BaseResponseT<List<ProductUnitDto>> response = new BaseResponseT<List<ProductUnitDto>>();
            try
            {
                var productUnits = productUnitRepository
                    .Get(product => product.ItemId == productId).ToList();
                if (productUnits != null && productUnits.Count() > 0)
                {
                    response.Result = mapper.Map<List<ProductUnitDto>>(productUnits);

                }
                else
                {
                    response.Result = new List<ProductUnitDto>();

                }


                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }

    }
}
