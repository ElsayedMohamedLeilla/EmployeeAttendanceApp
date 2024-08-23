using Dawem.Contract.BusinessLogic.Dawem.Provider;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Providers;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Providers;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Providers.Companies;
using Dawem.Models.Response.Dawem.Providers.CompanyBranches;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Provider
{
    public class CompanyBranchBL : ICompanyBranchBL
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public CompanyBranchBL(IRepositoryManager _repositoryManager, 
            RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<GetCompanyBranchByIdResponseModel> GetById(int branchId)
        {
            var branch = await repositoryManager.CompanyBranchRepository.
                Get(branch => branch.Id == branchId && 
                branch.CompanyId == requestInfo.CompanyId)
                .Select(company => new GetCompanyBranchByIdResponseModel
                {
                    Id = company.Id,
                    Name = company.Name,
                    Latitude = company.Latitude,
                    Longitude = company.Longitude
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryBranchNotFound);

            return branch;
        }
        public async Task<GetCompanyBranchesForDropDownResponse> GetForDropDown(GetCompanyBranchesCriteria criteria)
        {
            criteria.IsActive = true;
            var companyBranchRepository = repositoryManager.CompanyBranchRepository;
            var query = companyBranchRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = companyBranchRepository.OrderBy(query, nameof(CompanyBranch.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var branchesList = await queryPaged.Select(e => new GetCompanyBranchesForDropDownResponseModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetCompanyBranchesForDropDownResponse
            {
                Branches = branchesList,
                TotalCount = await query.CountAsync()
            };

            #endregion
        }
    }
}

