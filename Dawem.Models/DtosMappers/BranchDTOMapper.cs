using Dawem.Domain.Entities.Provider;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.DtosMappers
{
    public class BranchDTOMapper
    {
        private static RequestInfo userContext;
        public static void InitBranchContext(RequestInfo _userContext)
        {
            userContext = _userContext;
        }
        public static BranchDTO Map(Branch branch)
        {
            if (branch == null) return null;
            var DTO = new BranchDTO()
            {
                Id = branch.Id,
                CompanyId = branch.CompanyId,
                BranchName = branch.Name,
                AddedDate = branch.AddedDate,
                Address = branch.Address,
                AddUserId = branch.AddUserId,
                IsActive = branch.IsActive,
                CountryId = branch.CountryId,
                Email = branch.Email,
                IsMainBranch = branch.IsMainBranch,
                PhoneNumber = branch.PhoneNumber,
                CountryGlobalName = userContext.Lang == "ar" ? branch?.Country?.NameAr : branch?.Country?.NameEn
            };
            return DTO;
        }
        public static Branch Map(BranchDTO BranchDTO)
        {
            if (BranchDTO == null) return null;
            var _branch = new Branch()
            {
                Id = BranchDTO.Id,
                CompanyId = BranchDTO.CompanyId,
                Name = BranchDTO.BranchName,
                AddedDate = BranchDTO.AddedDate,
                Address = BranchDTO.Address,
                AddUserId = BranchDTO.AddUserId,
                IsActive = BranchDTO.IsActive,
                CountryId = BranchDTO.CountryId,
                Email = BranchDTO.Email,
                IsMainBranch = BranchDTO.IsMainBranch,
                PhoneNumber = BranchDTO.PhoneNumber
            };
            return _branch;
        }

        public static List<BranchDTO> Map(List<Branch> branches)
        {
            if (branches == null) return null;
            return branches.Select(selector: Map).ToList();
        }
    }
}
