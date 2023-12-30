using Dawem.Contract.BusinessLogic.Requests;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others;
using Dawem.Models.Dtos.Requests;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Requests;
using Dawem.Models.Response.Requests.Requests;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Requests
{
    public class RequestBL : IRequestBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IUploadBLC uploadBLC;
        public RequestBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IUploadBLC _uploadBLC,
           RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            uploadBLC = _uploadBLC;
        }
        public async Task<GetRequestsResponse> Get(GetRequestsCriteria criteria)
        {
            var requestRepository = repositoryManager.RequestRepository;
            var query = requestRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestRepository.OrderBy(query, nameof(Request.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestsList = await queryPaged.Select(request => new GetRequestsResponseModel
            {
                Id = request.Id,
                Code = request.Code,
                Employee = new RequestEmployeeModel
                {
                    Code = request.Employee.Code,
                    Name = request.Employee.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(request.Employee.ProfileImageName, LeillaKeys.Employees)
                },
                RequestType = request.Type,
                RequestTypeName = TranslationHelper.GetTranslation(request.Type.ToString(), requestInfo.Lang),
                Date = request.Date,
                Status = request.Status,
                StatusName = TranslationHelper.GetTranslation(request.Status.ToString(), requestInfo.Lang)

            }).ToListAsync();

            return new GetRequestsResponse
            {
                Requests = requestsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<EmployeeGetRequestsResponse> EmployeeGet(EmployeeGetRequestsCriteria criteria)
        {
            var requestRepository = repositoryManager.RequestRepository;
            var query = requestRepository.EmployeeGetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = requestRepository.OrderBy(query, nameof(Request.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var requestsList = await queryPaged.Select(request => new EmployeeGetRequestsResponseModel
            {
                Id = request.Id,
                Code = request.Code,
                DirectManagerName = request.Employee.DirectManager != null ?
                request.Employee.DirectManager.Name : null,
                RequestType = request.Type,
                RequestTypeName = TranslationHelper.GetTranslation(request.Type.ToString(), requestInfo.Lang) 
                + LeillaKeys.SpaceThenDashThenSpace + ( request.RequestVacation != null ? request.RequestVacation.VacationType.Name :
                request.RequestAssignment != null ? request.RequestAssignment.AssignmentType.Name :
                request.RequestJustification != null ? request.RequestJustification.JustificatioType.Name :
                request.RequestPermission != null ? request.RequestPermission.PermissionType.Name :
                request.RequestTask != null ? request.RequestTask.TaskType.Name : null),
                DateFrom = request.Date,
                DateTo= request.RequestVacation != null ? request.RequestVacation.DateTo :
                request.RequestAssignment != null ? request.RequestAssignment.DateTo :
                request.RequestJustification != null ? request.RequestJustification.DateTo :
                request.RequestPermission != null ? request.RequestPermission.DateTo :
                request.RequestTask != null ? request.RequestTask.DateTo : null,
                NumberOfDays = request.RequestVacation != null ? request.RequestVacation.NumberOfDays : null,
                BalanceBeforeRequest = request.RequestVacation != null ? request.RequestVacation.BalanceBeforeRequest : null,
                BalanceAfterRequest = request.RequestVacation != null ? request.RequestVacation.BalanceAfterRequest : null,
                Status = request.Status,
                StatusName = TranslationHelper.GetTranslation(request.Status.ToString(), requestInfo.Lang),
                AddedDate = request.AddedDate

            }).ToListAsync();

            return new EmployeeGetRequestsResponse
            {
                Requests = requestsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetRequestInfoResponseModel> GetInfo(int requestId)
        {
            var request = await repositoryManager.RequestRepository.Get(e => e.Id == requestId && !e.IsDeleted)
                .Select(request => new GetRequestInfoResponseModel
                {
                    Code = request.Code,
                    Employee = new RequestEmployeeModel
                    {
                        Code = request.Employee.Code,
                        Name = request.Employee.Name,
                        ProfileImagePath = uploadBLC.GetFilePath(request.Employee.ProfileImageName, LeillaKeys.Employees)
                    },
                    RequestType = request.Type,
                    RequestTypeName = TranslationHelper.GetTranslation(request.Type.ToString(), requestInfo.Lang),
                    Date = request.Date,
                    Status = request.Status,
                    StatusName = TranslationHelper.GetTranslation(request.Status.ToString(), requestInfo.Lang),
                    Attachments = request.RequestAttachments
                    .Select(a => new FileDTO
                    {
                        FileName = a.FileName,
                        FilePath = uploadBLC.GetFilePath(a.FileName, GetFolderName(request.Type))
                    }).ToList(),
                    Notes = request.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            return request;
        }
        private static string GetFolderName(RequestType type)
        {
            return type switch
            {
                RequestType.Assignment => LeillaKeys.AssignmentRequests,
                RequestType.Justification => AmgadKeys.JustificationRequests,
                RequestType.Permission => LeillaKeys.PermissionRequests,
                RequestType.Task => LeillaKeys.TaskRequests,
                RequestType.Vacation => LeillaKeys.VacationRequests,
                _ => LeillaKeys.AssignmentRequests,
            };
        }
        public async Task<bool> Accept(int requestId)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
               throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            if (request.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyAccepted);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyRejected);
            }

            request.Status = RequestStatus.Accepted;
            request.DecisionUserId = requestInfo.UserId;
            request.DecisionDate = DateTime.UtcNow;

            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Reject(RejectModelDTO rejectModelDTO)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == rejectModelDTO.Id) ??
               throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            if (request.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyAccepted);
            }
            else if (request.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestAlreadyRejected);
            }

            request.Status = RequestStatus.Rejected;
            request.DecisionUserId = requestInfo.UserId;
            request.DecisionDate = DateTime.UtcNow;
            request.RejectReason = rejectModelDTO.RejectReason;

            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int requestId)
        {
            var request = await repositoryManager.RequestRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == requestId) ??
                throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);
            request.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetRequestsInformationsResponseDTO> GetRequestsInformations()
        {
            var requestRepository = repositoryManager.RequestRepository;
            var query = requestRepository.Get(request => !request.IsDeleted &&
            request.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetRequestsInformationsResponseDTO
            {
                TotalRequestsCount = await query.CountAsync(),
                AcceptedCount = await query.Where(request => request.Status == RequestStatus.Accepted).CountAsync(),
                RejectedCount = await query.Where(request => request.Status == RequestStatus.Rejected).CountAsync(),
                PendingCount = await query.Where(request => request.Status == RequestStatus.Pending).CountAsync()
            };

            #endregion
        }
    }
}

