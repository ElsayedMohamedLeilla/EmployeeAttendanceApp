using AutoMapper;
using Dawem.Contract.BusinessLogic.WeekAttendances;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Employees.Attendances.WeeksAttendances;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.WeekAttendances
{
    public class WeekAttendanceBL : IWeekAttendanceBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IWeekAttendanceBLValidation weekAttendanceBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public WeekAttendanceBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IWeekAttendanceBLValidation _weekAttendanceBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            weekAttendanceBLValidation = _weekAttendanceBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateWeekAttendanceModel model)
        {
            #region Business Validation

            await weekAttendanceBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert WeekAttendance

            #region Set WeekAttendance code

            var getNextCode = await repositoryManager.WeekAttendanceRepository
                .Get(weekAttendance => weekAttendance.CompanyId == requestInfo.CompanyId)
                .Select(weekAttendance => weekAttendance.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var weekAttendance = mapper.Map<WeekAttendance>(model);
            weekAttendance.CompanyId = requestInfo.CompanyId;
            weekAttendance.AddUserId = requestInfo.UserId;
            weekAttendance.Code = getNextCode;
            repositoryManager.WeekAttendanceRepository.Insert(weekAttendance);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return weekAttendance.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateWeekAttendanceModel model)
        {
            #region Business Validation

            await weekAttendanceBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update WeekAttendance

            var getWeekAttendance = await repositoryManager.WeekAttendanceRepository
                .GetEntityByConditionWithTrackingAsync(weekAttendance => !weekAttendance.IsDeleted
            && weekAttendance.Id == model.Id);

            getWeekAttendance.Name = model.Name;
            getWeekAttendance.Notes = model.Notes;
            getWeekAttendance.IsActive = model.IsActive;
            getWeekAttendance.ModifiedDate = DateTime.Now;
            getWeekAttendance.ModifyUserId = requestInfo.UserId;

            await unitOfWork.SaveAsync();

            #region Handle Week Shifts

            var getDBWeekShifts = await repositoryManager.WeekAttendanceShiftRepository
                .GetWithTracking(w => w.WeekAttendanceId == model.Id)
                .ToListAsync();

            foreach (var getDBWeekShift in getDBWeekShifts)
            {
                getDBWeekShift.ShiftId = model?.WeekShifts?.FirstOrDefault(w => w.WeekDay == getDBWeekShift.WeekDay)?.ShiftId;
            }

            await unitOfWork.SaveAsync();

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetWeekAttendancesResponse> Get(GetWeekAttendancesCriteria criteria)
        {
            var weekAttendanceRepository = repositoryManager.WeekAttendanceRepository;
            var query = weekAttendanceRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = weekAttendanceRepository.OrderBy(query, nameof(WeekAttendance.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var weekAttendancesList = await queryPaged.Select(weekAttendance => new GetWeekAttendancesResponseModel
            {
                Id = weekAttendance.Id,
                Code = weekAttendance.Code,
                Name = weekAttendance.Name,
                IsActive = weekAttendance.IsActive
            }).ToListAsync();

            return new GetWeekAttendancesResponse
            {
                WeekAttendances = weekAttendancesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetWeekAttendancesForDropDownResponse> GetForDropDown(GetWeekAttendancesCriteria criteria)
        {
            criteria.IsActive = true;
            var weekAttendanceRepository = repositoryManager.WeekAttendanceRepository;
            var query = weekAttendanceRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = weekAttendanceRepository.OrderBy(query, nameof(WeekAttendance.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var weekAttendancesList = await queryPaged.Select(weekAttendance => new GetWeekAttendancesForDropDownResponseModel
            {
                Id = weekAttendance.Id,
                Name = weekAttendance.Name
            }).ToListAsync();

            return new GetWeekAttendancesForDropDownResponse
            {
                WeekAttendances = weekAttendancesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetWeekAttendanceInfoResponseModel> GetInfo(int weekAttendanceId)
        {
            var weekAttendance = await repositoryManager.WeekAttendanceRepository.Get(weekAttendance => weekAttendance.Id == weekAttendanceId && !weekAttendance.IsDeleted)
                .Select(weekAttendance => new GetWeekAttendanceInfoResponseModel
                {
                    Code = weekAttendance.Code,
                    Name = weekAttendance.Name,
                    Notes = weekAttendance.Notes,
                    IsActive = weekAttendance.IsActive,
                    WeekShifts = weekAttendance.WeekAttendanceShifts.Select(weekShift => new WeekAttendanceShiftTextModel
                    {
                        WeekDayName = TranslationHelper.GetTranslation(weekShift.WeekDay.ToString(), requestInfo.Lang),
                        ShiftName = weekShift.Shift != null ? weekShift.Shift.Name : null
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryWeekAttendanceNotFound);

            return weekAttendance;
        }
        public async Task<GetWeekAttendanceByIdResponseModel> GetById(int weekAttendanceId)
        {
            var weekAttendance = await repositoryManager.WeekAttendanceRepository.Get(weekAttendance => weekAttendance.Id == weekAttendanceId && !weekAttendance.IsDeleted)
                .Select(weekAttendance => new GetWeekAttendanceByIdResponseModel
                {
                    Id = weekAttendance.Id,
                    Code = weekAttendance.Code,
                    Name = weekAttendance.Name,
                    IsActive = weekAttendance.IsActive,
                    WeekShifts = weekAttendance.WeekAttendanceShifts.Select(weekShift => new WeekAttendanceShiftUpdateModel
                    {
                        Id = weekShift.Id,
                        WeekDay = weekShift.WeekDay,
                        ShiftId = weekShift.ShiftId,
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryWeekAttendanceNotFound);

            return weekAttendance;

        }
        public async Task<bool> Delete(int weekAttendanceId)
        {
            var weekAttendance = await repositoryManager.WeekAttendanceRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == weekAttendanceId) ??
                throw new BusinessValidationException(LeillaKeys.SorryWeekAttendanceNotFound);

            weekAttendance.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}