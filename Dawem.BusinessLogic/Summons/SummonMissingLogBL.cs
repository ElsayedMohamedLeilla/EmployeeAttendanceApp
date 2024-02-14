using Dawem.Contract.BusinessLogic.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Summons;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Summons.Summons;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Summons
{
    public class SummonMissingLogBL : ISummonMissingLogBL
    {
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        public SummonMissingLogBL(IRepositoryManager _repositoryManager, RequestInfo _requestHeaderContext)
        {
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
        }
        public async Task<GetSummonMissingLogsResponse> Get(GetSummonMissingLogsCriteria criteria)
        {
            var summonMissingLogRepository = repositoryManager.SummonMissingLogRepository;
            var query = summonMissingLogRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = summonMissingLogRepository.OrderBy(query, nameof(SummonMissingLog.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var summonMissingLogsList = await queryPaged.Select(sml => new GetSummonMissingLogsResponseModel
            {
                Id = sml.Id,
                Code = sml.Code,
                EmployeeName = sml.Employee.Name,
                SummonCode = sml.Summon.Code.ToString(),
                SummonDate = sml.Summon.DateAndTime,
                SanctionsCount = sml.SummonMissingLogSanctions.Count
            }).ToListAsync();

            return new GetSummonMissingLogsResponse
            {
                SummonMissingLogs = summonMissingLogsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetSummonMissingLogInfoResponseModel> GetInfo(int summonMissingLogId)
        {
            var summonMissingLog = await repositoryManager.SummonMissingLogRepository.Get(e => e.Id == summonMissingLogId && !e.IsDeleted)
                .Select(sml => new GetSummonMissingLogInfoResponseModel
                {
                    Code = sml.Code,
                    EmployeeName = sml.Employee.Name,
                    SummonCode = sml.Summon.Code.ToString(),
                    SummonDate = sml.Summon.DateAndTime,
                    SanctionsCount = sml.SummonMissingLogSanctions.Count,
                    SummonSanctions = sml.SummonMissingLogSanctions.Select(smls => smls.SummonSanction.Sanction.Name).ToList(),
                    SummonForTypeName = TranslationHelper.GetTranslation(sml.Summon.ForType.ToString(), requestInfo.Lang),
                    SummonAllowedTimeName = sml.Summon.AllowedTime + LeillaKeys.Space + TranslationHelper.GetTranslation(sml.Summon.TimeType.ToString() + LeillaKeys.TimeType, requestInfo.Lang)
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySummonMissingLogNotFound);

            return summonMissingLog;
        }
        public async Task<GetSummonMissingLogsInformationsResponseDTO> GetSummonMissingLogsInformations()
        {
            var summonMissingLogRepository = repositoryManager.SummonMissingLogRepository;
            var query = summonMissingLogRepository.Get(summonMissingLog => summonMissingLog.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetSummonMissingLogsInformationsResponseDTO
            {
                TotalCount = await query.Where(summonMissingLog => !summonMissingLog.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(summonMissingLog => !summonMissingLog.IsDeleted && summonMissingLog.IsActive).CountAsync(),
                NotActiveCount = await query.Where(summonMissingLog => !summonMissingLog.IsDeleted && !summonMissingLog.IsActive).CountAsync(),
                DeletedCount = await query.Where(summonMissingLog => summonMissingLog.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

