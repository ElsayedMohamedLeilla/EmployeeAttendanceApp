using AutoMapper;
using Dawem.BusinessLogicCore;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessLogicCore;
using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Group;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Dtos.Employees.Employees.GroupEmployees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.Groups;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace Dawem.BusinessLogic.Core.Groups
{
    public class GroupBL : IGroupBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IGroupBLValidation GroupBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;


        public GroupBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
         IUploadBLC _uploadBLC,
        IGroupBLValidation _GroupBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            GroupBLValidation = _GroupBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;

        }
        public async Task<int> Create(CreateGroupDTO model)
        {
            #region assign EmployeeIdes In GroupEmployees Object
            model.MapGroupEmployees();
            #endregion
            #region assign DelegatorsIdes In GroupManagerDelegators Object
            model.MapGroupManagarDelegators();
            #endregion
            #region Business Validation
            await GroupBLValidation.CreateValidation(model);
            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Group

            #region Set Group code
            var getNextCode = await repositoryManager.GroupRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var Group = mapper.Map<Group>(model);
            Group.CompanyId = requestInfo.CompanyId;
            Group.AddUserId = requestInfo.UserId;
            Group.AddedApplicationType = requestInfo.ApplicationType;
            Group.Code = getNextCode;
            repositoryManager.GroupRepository.Insert(Group);
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response
            await unitOfWork.CommitAsync();
            return Group.Id;
            #endregion

        }
        public async Task<bool> Update(UpdateGroupDTO model)
        {
            #region assign EmployeeIdes In GroupEmployees Object
            model.MapGroupEmployees();
            #endregion
            #region assign EmployeeIdes In GroupEmployees Object
            model.MapGroupManagarDelegators();
            #endregion
            #region Business Validation

            await GroupBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();
            #region Update Group
            var getGroup = await repositoryManager.GroupRepository.GetByIdAsync(model.Id);
            getGroup.Name = model.Name;
            getGroup.IsActive = model.IsActive;
            getGroup.ModifiedDate = DateTime.Now;
            getGroup.ModifyUserId = requestInfo.UserId;
            getGroup.GroupManagerId = model.GroupManagerId;
            getGroup.ModifiedApplicationType = requestInfo.ApplicationType;
            await unitOfWork.SaveAsync();
            #endregion

            #region Update GroupEmployees
            if (getGroup != null)
            {
                List<GroupEmployee> existDbList = repositoryManager.GroupEmployeeRepository
                    .GetByCondition(e => e.GroupId == getGroup.Id)
                    .ToList();

                List<int> existingEmployeeIds = existDbList.Select(e => e.EmployeeId).ToList();

                List<GroupEmployee> addedGroupEmployees = model.GroupEmployees
                    .Where(ge => !existingEmployeeIds.Contains(ge.EmployeeId))
                    .Select(ge => new GroupEmployee
                    {
                        GroupId = model.Id,
                        EmployeeId = ge.EmployeeId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    })
                    .ToList();

                List<int> employeesToRemove = existDbList
                    .Where(ge => !model.EmployeeIdes.Contains(ge.EmployeeId))
                    .Select(ge => ge.EmployeeId)
                    .ToList();

                List<GroupEmployee> removedGroupEmployees = repositoryManager.GroupEmployeeRepository
                    .GetByCondition(e => e.GroupId == model.Id && employeesToRemove.Contains(e.EmployeeId))
                    .ToList();

                if (removedGroupEmployees.Count > 0)
                    repositoryManager.GroupEmployeeRepository.BulkDeleteIfExist(removedGroupEmployees);
                if (addedGroupEmployees.Count > 0)
                    repositoryManager.GroupEmployeeRepository.BulkInsert(addedGroupEmployees);
            }
            #endregion

            #region Update GroupManagerDelgators
            if (getGroup != null)
            {
                List<GroupManagerDelegator> ExistDbList = repositoryManager.GroupManagerDelegatorRepository
                    .GetByCondition(e => e.GroupId == getGroup.Id)
                    .ToList();

                List<int> existingEmployeeIds = ExistDbList.Select(e => e.EmployeeId).ToList();

                List<GroupManagerDelegator> addedGroupManagerDelegators = model.GroupManagerDelegators
                    .Where(gmd => !existingEmployeeIds.Contains(gmd.EmployeeId))
                    .Select(gmd => new GroupManagerDelegator
                    {
                        GroupId = model.Id,
                        EmployeeId = gmd.EmployeeId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    }).ToList();

                List<int> groupManagerDelegatorToRemove = ExistDbList
                    .Where(gmd => !model.GroupManagerDelegatorIdes.Contains(gmd.EmployeeId))
                    .Select(gmd => gmd.EmployeeId)
                    .ToList();

                List<GroupManagerDelegator> removedgroupManagerDelegators = repositoryManager.GroupManagerDelegatorRepository
                    .GetByCondition(e => e.GroupId == model.Id && groupManagerDelegatorToRemove.Contains(e.EmployeeId))
                    .ToList();
                if (removedgroupManagerDelegators.Count > 0)
                    repositoryManager.GroupManagerDelegatorRepository.BulkDeleteIfExist(removedgroupManagerDelegators);
                if (addedGroupManagerDelegators.Count > 0)
                    repositoryManager.GroupManagerDelegatorRepository.BulkInsert(addedGroupManagerDelegators);
            }
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetGroupResponseDTO> Get(GetGroupCriteria criteria)
        {
            var GroupRepository = repositoryManager.GroupRepository;
            var query = GroupRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = GroupRepository.OrderBy(query, nameof(Group.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response


            var GroupsList = await queryPaged.Select(group => new GroupEmployeeForGridDTO
            {
                Id = group.Id,
                Code = group.Code,
                Name = group.Name,
                IsActive = group.IsActive,
                GroupManager = new GroupManagarForGridDTO
                {
                    GroupManagerName = group.GroupManager != null ? group.GroupManager.Name : null,
                    ProfileImagePath = group.GroupManager != null ? uploadBLC.GetFilePath(group.GroupManager.ProfileImageName, LeillaKeys.Employees) : null ,
                }

            }).ToListAsync();

          

            return new GetGroupResponseDTO
            {
                Groups = GroupsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetGroupDropDownResponseDTO> GetForDropDown(GetGroupCriteria criteria)
        {
            criteria.IsActive = true;
            var GroupRepository = repositoryManager.GroupRepository;
            var query = GroupRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = GroupRepository.OrderBy(query, nameof(Group.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var GroupsList = await queryPaged.Select(e => new GetGroupForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetGroupDropDownResponseDTO
            {
                Groups = GroupsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetGroupInfoResponseDTO> GetInfo(int GroupId)
        {
            var Group = await repositoryManager.GroupRepository.Get(e => e.Id == GroupId && !e.IsDeleted)
     .Select(group => new GetGroupInfoResponseDTO
     {
         Code = group.Code,
         Name = group.Name,
         IsActive = group.IsActive,
         GroupEmployees = group.GroupEmployees
             .Join(repositoryManager.EmployeeRepository.GetAll(), // Assuming access to Employee repository
                 groupEmployee => groupEmployee.EmployeeId,
                 employee => employee.Id,
                 (groupEmployee, employee) => employee.Name) // Select employee names
             .ToList(),
         GroupManagerDelegators = group.GroupManagerDelegators
             .Join(repositoryManager.EmployeeRepository.GetAll(), // Assuming access to Employee repository
                 groupEmployee => groupEmployee.EmployeeId,
                 employee => employee.Id,
                 (groupEmployee, employee) => employee.Name) // Select employee names
             .ToList(),
         GroupManager = group.GroupManager.Name
                       
     }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryGroupNotFound);

            return Group;
        }
        public async Task<GetGroupByIdResponseDTO> GetById(int GroupId)
        {
            var Group = await repositoryManager.GroupRepository.Get(e => e.Id == GroupId && !e.IsDeleted)
                .Select(group => new GetGroupByIdResponseDTO
                {
                    Id = group.Id,
                    Code = group.Code,
                    Name = group.Name,
                    IsActive = group.IsActive,
                    GroupManagerId=group.GroupManagerId,
                    EmployeeIdes = group.GroupEmployees
             .Join(repositoryManager.EmployeeRepository.GetAll(), 
                 groupEmployee => groupEmployee.EmployeeId,
                 employee => employee.Id,
                 (groupEmployee, employee) => employee.Id)
             .ToList(),
                    GroupManagerDelegatorIdes = group.GroupManagerDelegators
             .Join(repositoryManager.EmployeeRepository.GetAll(),
                 groupEmployee => groupEmployee.EmployeeId,
                 employee => employee.Id,
                 (groupEmployee, employee) => employee.Id)
             .ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryGroupNotFound);

            return Group;

        }
        public async Task<bool> Delete(int GroupId)
        {
            var Group = await repositoryManager.GroupRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == GroupId) ??
                throw new BusinessValidationException(AmgadKeys.SorryGroupNotFound);

            Group.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int GroupId)
        {
            var group = await repositoryManager.GroupRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == GroupId) ??
                throw new BusinessValidationException(AmgadKeys.SorryGroupNotFound);
            group.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.GroupRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryGroupNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }

    }
}
