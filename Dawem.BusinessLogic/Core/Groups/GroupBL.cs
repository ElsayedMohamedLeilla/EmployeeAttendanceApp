using AutoMapper;
using Dawem.Contract.BusinessLogic.Core;
using Dawem.Contract.BusinessValidation.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Core.Groups;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Core.Groups;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Core.Groups
{
    public class GroupBL : IGroupBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IGroupBLValidation GroupBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public GroupBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IGroupBLValidation _GroupBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            GroupBLValidation = _GroupBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateGroupDTO model)
        {
            #region assign EmployeeIdes In GroupEmployees Object
            List<GroupEmployeeCreateModelDTO> insertedList = new List<GroupEmployeeCreateModelDTO>();
            foreach (var employee in model.EmployeeIdes)
            {
                GroupEmployeeCreateModelDTO temp = new GroupEmployeeCreateModelDTO();
                temp.EmployeeId = employee;
                insertedList.Add(temp);

            }
            model.GroupEmployees = insertedList;
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
            List<GroupEmployeeUpdateModelDTO> insertedList = new List<GroupEmployeeUpdateModelDTO>();
            foreach (var employee in model.EmployeeIdes)
            {
                GroupEmployeeUpdateModelDTO temp = new GroupEmployeeUpdateModelDTO();
                temp.EmployeeId = employee;
                insertedList.Add(temp);

            }
            model.GroupEmployees = insertedList;
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
            await unitOfWork.SaveAsync();
            #endregion

            #region Update GroupEmployees
            if (getGroup != null)
            {

                List<GroupEmployee> ExistDbList = repositoryManager.GroupEmployeeRepository.GetByCondition(e => e.GroupId == getGroup.Id).ToList();
                List<int> employeesToRemove = new List<int>();
                List<GroupEmployee> addedGroupEmployees = new List<GroupEmployee>();

                for (int i = 0; i < model.GroupEmployees.Count; i++)
                {
                    int employeeId = model.GroupEmployees[i].EmployeeId; // Assuming model.GroupEmployees contains employee IDs

                    // Check if the employee ID is not present in the existing list, add it to employeesToAdd
                    if (!ExistDbList.Any(e => e.EmployeeId == employeeId))
                    {
                        //employeesToAdd.Add(employeeId);
                        GroupEmployee temp = new GroupEmployee()
                        {
                            GroupId = model.Id,
                            EmployeeId = employeeId,
                            ModifyUserId = requestInfo.UserId,
                            ModifiedDate = DateTime.UtcNow
                        };
                        addedGroupEmployees.Add(temp);
                    }
                }
                foreach (var groupEmployee in ExistDbList)
                {
                    // Check if the employee ID in ExistDbList is not present in model.GroupEmployees
                    if (!model.EmployeeIdes.Contains(groupEmployee.EmployeeId))
                    {
                        employeesToRemove.Add(groupEmployee.EmployeeId);
                    }
                }
                List<GroupEmployee> removedgroupEmployees = repositoryManager.GroupEmployeeRepository
               .GetByCondition(e => e.GroupId == model.Id && employeesToRemove.Contains(e.EmployeeId))
               .ToList();
                // remove useless
                repositoryManager.GroupEmployeeRepository.BulkDeleteIfExist(removedgroupEmployees);
                //add new 
                repositoryManager.GroupEmployeeRepository.BulkInsert(addedGroupEmployees);

            }



            #endregion
            // await unitOfWork.SaveAsync();

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

            var GroupsList = await queryPaged.Select(group => new GetGroupResponseModelDTO
            {
                Id = group.Id,
                Code = group.Code,
                Name = group.Name,
                IsActive = group.IsActive,
               


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
                    EmployeeIdes = group.GroupEmployees
             .Join(repositoryManager.EmployeeRepository.GetAll(), // Assuming access to Employee repository
                 groupEmployee => groupEmployee.EmployeeId,
                 employee => employee.Id,
                 (groupEmployee, employee) => employee.Id) // Select employee names
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

    }
}
