using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.Groups;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupManagarDelegators;
using Dawem.Models.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.Groups;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.Groups
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
            #region assign ZoneIds In GroupZones Object
            if (model.ZoneIds != null && model.ZoneIds.Count > 0)
                model.MapGroupZones();
            #endregion
            #region assign EmployeeIdes In GroupEmployees Object
            if (model.EmployeeIds != null && model.EmployeeIds.Count > 0)
                model.MapGroupEmployees();
            #endregion
            #region assign DelegatorsIdes In GroupManagerDelegators Object
            if (model.ManagerDelegatorIds != null && model.ManagerDelegatorIds.Count > 0)
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
            #region assign ZoneIds In GroupZones Object
            model.MapGroupZones();
            #endregion
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

            var getGroup = await repositoryManager.GroupRepository.GetEntityByConditionWithTrackingAsync(grp => !grp.IsDeleted
            && grp.Id == model.Id);
            if (getGroup != null)
            {
                getGroup.Name = model.Name;
                getGroup.IsActive = model.IsActive;
                getGroup.ModifiedDate = DateTime.UtcNow;
                getGroup.ModifyUserId = requestInfo.UserId;
                getGroup.ManagerId = model.ManagerId;
                getGroup.ModifiedApplicationType = requestInfo.ApplicationType;
                #endregion

                #region Update GroupEmployees

                List<GroupEmployee> existDbList = repositoryManager.GroupEmployeeRepository
                    .GetByCondition(e => e.GroupId == getGroup.Id)
                    .ToList();

                List<int> existingEmployeeIds = existDbList.Select(e => e.EmployeeId).ToList();

                List<GroupEmployee> addedGroupEmployees = model.Employees
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
                    .Where(ge => !model.EmployeeIds.Contains(ge.EmployeeId))
                    .Select(ge => ge.EmployeeId)
                    .ToList();

                List<GroupEmployee> removedGroupEmployees = repositoryManager.GroupEmployeeRepository
                    .GetByCondition(e => e.GroupId == model.Id && employeesToRemove.Contains(e.EmployeeId))
                    .ToList();

                if (removedGroupEmployees.Count > 0)
                    repositoryManager.GroupEmployeeRepository.BulkDeleteIfExist(removedGroupEmployees);
                if (addedGroupEmployees.Count > 0)
                    repositoryManager.GroupEmployeeRepository.BulkInsert(addedGroupEmployees);

                #endregion


                #region Update GroupManagerDelgators

                List<GroupManagerDelegator> ExistDbList = repositoryManager.GroupManagerDelegatorRepository
                    .GetByCondition(e => e.GroupId == getGroup.Id)
                    .ToList();

                List<int> existingManagerIds = ExistDbList.Select(e => e.EmployeeId).ToList();

                List<GroupManagerDelegator> addedGroupManagerDelegators = model.ManagerDelegators
                    .Where(gmd => !existingManagerIds.Contains(gmd.EmployeeId))
                    .Select(gmd => new GroupManagerDelegator
                    {
                        GroupId = model.Id,
                        EmployeeId = gmd.EmployeeId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    }).ToList();

                List<int> groupManagerDelegatorToRemove = ExistDbList
                    .Where(gmd => !model.ManagerDelegatorIds.Contains(gmd.EmployeeId))
                    .Select(gmd => gmd.EmployeeId)
                    .ToList();

                List<GroupManagerDelegator> removedgroupManagerDelegators = repositoryManager.GroupManagerDelegatorRepository
                    .GetByCondition(e => e.GroupId == model.Id && groupManagerDelegatorToRemove.Contains(e.EmployeeId))
                    .ToList();
                if (removedgroupManagerDelegators.Count > 0)
                    repositoryManager.GroupManagerDelegatorRepository.BulkDeleteIfExist(removedgroupManagerDelegators);
                if (addedGroupManagerDelegators.Count > 0)
                    repositoryManager.GroupManagerDelegatorRepository.BulkInsert(addedGroupManagerDelegators);

                #endregion

                #region Update ZoneGroup

                List<ZoneGroup> existZDbList = repositoryManager.ZoneGroupRepository
                        .GetByCondition(e => e.GroupId == getGroup.Id)
                        .ToList();

                List<int> existingZoneIds = existZDbList.Select(e => e.ZoneId).ToList();

                var addedGroupZones = model.Zones != null ? model.Zones
                    .Where(ge => !existingZoneIds.Contains(ge.ZoneId))
                    .Select(ge => new ZoneGroup
                    {
                        GroupId = model.Id,
                        ZoneId = ge.ZoneId,
                        ModifyUserId = requestInfo.UserId,
                        ModifiedDate = DateTime.UtcNow
                    }).ToList() : null;

                List<int> ZonesToRemove = existZDbList
                    .Where(ge => model.ZoneIds == null || model.ZoneIds.Contains(ge.ZoneId))
                    .Select(ge => ge.ZoneId)
                    .ToList();

                List<ZoneGroup> removedGroupZones = repositoryManager.ZoneGroupRepository
                    .GetByCondition(e => e.GroupId == model.Id && ZonesToRemove.Contains(e.ZoneId))
                    .ToList();

                if (removedGroupZones.Count > 0)
                    repositoryManager.ZoneGroupRepository.BulkDeleteIfExist(removedGroupZones);
                if (addedGroupZones.Count > 0)
                    repositoryManager.ZoneGroupRepository.BulkInsert(addedGroupZones);

                #endregion
            }

            else
                throw new BusinessValidationException(AmgadKeys.SorryGroupNotFound);

            await unitOfWork.SaveAsync();
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

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response


            var GroupsList = await queryPaged.Select(group => new GroupEmployeeForGridDTO
            {
                Id = group.Id,
                Code = group.Code,
                Name = group.Name,
                IsActive = group.IsActive,
                NumberOfEmployees = group.GroupEmployees.Count,
                Manager = group.ManagerId != null ? new GroupManagarForGridDTO
                {
                    ManagerName = group.GroupManager.Name,
                    ProfileImagePath = uploadBLC.GetFilePath(group.GroupManager.ProfileImageName, LeillaKeys.Employees),
                } : null

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

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

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
         Employees = group.GroupEmployees
             .Join(repositoryManager.EmployeeRepository.GetAll(), // Assuming access to Employee repository
                 groupEmployee => groupEmployee.EmployeeId,
                 employee => employee.Id,
                 (groupEmployee, employee) => employee.Name) // Select employee names
             .ToList(),
         ManagerDelegators = group.GroupManagerDelegators
             .Join(repositoryManager.EmployeeRepository.GetAll(), // Assuming access to Employee repository
                 groupEmployee => groupEmployee.EmployeeId,
                 employee => employee.Id,
                 (groupEmployee, employee) => employee.Name) // Select employee names
             .ToList(),
         ManagerName = group.GroupManager.Name,
         Zones = group.Zones
             .Join(repositoryManager.ZoneRepository.GetAll(), // Assuming access to Employee repository
                 depZone => depZone.ZoneId,
                 zone => zone.Id,
                 (zoneGroup, zone) => zone.Name) // Select Zone names
             .ToList()

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
                    ManagerId = group.ManagerId,
                    EmployeeIds = group.GroupEmployees
                    .Join(repositoryManager.EmployeeRepository.GetAll(),
                    groupEmployee => groupEmployee.EmployeeId, employee => employee.Id,
                    (groupEmployee, employee) => employee.Id).ToList(),
                    ManagerDelegatorIds = group.GroupManagerDelegators.Join(repositoryManager.EmployeeRepository.GetAll(),
                    groupEmployee => groupEmployee.EmployeeId,
                    employee => employee.Id,
                    (groupEmployee, employee) => employee.Id).ToList(),
                    ZoneIds = group.Zones.Join(repositoryManager.ZoneRepository.GetAll(),
                    zoneDepartment => zoneDepartment.ZoneId,
                    zone => zone.Id,
                    (zoneDepartment, zone) => zone.Id).ToList(),
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
        public async Task<GetGroupsInformationsResponseDTO> GetGroupsInformations()
        {
            var groupRepository = repositoryManager.GroupRepository;
            var query = groupRepository.Get(group => group.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetGroupsInformationsResponseDTO
            {
                TotalCount = await query.Where(group => !group.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(group => !group.IsDeleted && group.IsActive).CountAsync(),
                NotActiveCount = await query.Where(group => !group.IsDeleted && !group.IsActive).CountAsync(),
                DeletedCount = await query.Where(group => group.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}
