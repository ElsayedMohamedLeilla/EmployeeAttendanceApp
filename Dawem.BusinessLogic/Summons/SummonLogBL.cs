using Dawem.Contract.BusinessLogic.Summons;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Summons;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Summons.Summons;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Summons.Summons;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Summons
{
    public class SummonLogBL : ISummonLogBL
    {
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        public SummonLogBL(IRepositoryManager _repositoryManager, 
            RequestInfo _requestHeaderContext)
        {
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
        }
        public async Task<GetSummonLogsResponse> Get(GetSummonLogsCriteria criteria)
        {
            var summonLogRepository = repositoryManager.SummonLogRepository;
            var query = summonLogRepository.GetAsQueryable(criteria);
            var utcDate = DateTime.UtcNow;

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = summonLogRepository.OrderBy(query, nameof(SummonLog.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var summonLogsList = await queryPaged.Select(sml => new GetSummonLogsResponseModel
            {
                Id = sml.Id,
                EmployeeName = sml.Employee.Name,
                SummonCode = sml.Summon.Code.ToString(),
                SummonDate = sml.Summon.LocalDateAndTime,
                DoneSummon = sml.DoneSummon,
                DoneDate = sml.DoneDate,
                SummonStatus = utcDate > sml.Summon.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < sml.Summon.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing,
                SummonStatusName = TranslationHelper.GetTranslation(nameof(SummonStatus) + (utcDate > sml.Summon.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < sml.Summon.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing).ToString() + LeillaKeys.TimeType, requestInfo.Lang),
                SanctionsCount = sml.SummonLogSanctions.Count
            }).ToListAsync();

            return new GetSummonLogsResponse
            {
                SummonLogs = summonLogsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetSummonLogInfoResponseModel> GetInfo(int summonLogId)
        {
            var utcDate = DateTime.UtcNow;
            var summonLog = await repositoryManager.SummonLogRepository.Get(e => e.Id == summonLogId && !e.IsDeleted)
                .Select(sml => new GetSummonLogInfoResponseModel
                {
                    EmployeeName = sml.Employee.Name,
                    SummonCode = sml.Summon.Code.ToString(),
                    SummonDate = sml.Summon.LocalDateAndTime,
                    SanctionsCount = sml.SummonLogSanctions.Count,
                    SummonSanctions = sml.SummonLogSanctions.Select(smls => smls.SummonSanction.Sanction.Name).ToList(),
                    SummonForTypeName = TranslationHelper.GetTranslation(sml.Summon.ForType.ToString(), requestInfo.Lang),
                    SummonAllowedTimeName = sml.Summon.AllowedTime + LeillaKeys.Space + TranslationHelper.GetTranslation(sml.Summon.TimeType.ToString() + LeillaKeys.TimeType, requestInfo.Lang),
                    DoneSummon = sml.DoneSummon,
                    SummonStatus = utcDate > sml.Summon.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < sml.Summon.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing,
                    SummonStatusName = TranslationHelper.GetTranslation(nameof(SummonStatus) + (utcDate > sml.Summon.EndDateAndTimeUTC ?
                    SummonStatus.Finished : utcDate < sml.Summon.StartDateAndTimeUTC ?
                    SummonStatus.NotStarted : SummonStatus.OnGoing).ToString() + LeillaKeys.TimeType, requestInfo.Lang),
                    DoneDate = sml.DoneDate
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySummonLogNotFound);

            return summonLog;
        }
        public async Task<GetSummonLogsInformationsResponseDTO> GetSummonLogsInformations()
        {
            var summonLogRepository = repositoryManager.SummonLogRepository;
            var query = summonLogRepository.Get(summonLog => summonLog.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetSummonLogsInformationsResponseDTO
            {
                TotalCount = await query.Where(summonLog => !summonLog.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(summonLog => !summonLog.IsDeleted && summonLog.IsActive).CountAsync(),
                NotActiveCount = await query.Where(summonLog => !summonLog.IsDeleted && !summonLog.IsActive).CountAsync(),
                DeletedCount = await query.Where(summonLog => summonLog.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

