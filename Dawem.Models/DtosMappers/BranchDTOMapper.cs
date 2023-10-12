using Dawem.Domain.Entities.Provider;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Provider;

namespace SmartBusinessERP.Models.DtosMappers
{
    public class BranchDTOMapper
    {
        private static RequestHeaderContext? userContext;
        public static void InitBranchContext(RequestHeaderContext _userContext)
        {
            userContext = _userContext;
        }
        public static BranchDTO? Map(Branch? branch)
        {
            if (branch == null) return null;
            var DTO = new BranchDTO()
            {
                Id = branch.Id,
                CompanyId = branch.CompanyId,
                BranchName = branch.BranchName,
                AddedDate = branch.AddedDate,
                Address = branch.Address,
                AddUserId = branch.AddUserId,
                IsActive = branch.IsActive,
                CurrencyId = branch.CurrencyId,
                CountryId = branch.CountryId,
                Email = branch.Email,
                IsMainBranch = branch.IsMainBranch,
                PhoneNumber = branch.PhoneNumber,
                TaxRegistrationNumber = branch.TaxRegistrationNumber,
                CommercialRecordNumber = branch.CommercialRecordNumber,
                CountryGlobalName = userContext.Lang == "ar" ? branch?.Country?.NameAr : branch?.Country?.NameEn,
                CurrencyGlobalName = userContext.Lang == "ar" ? branch?.Currency?.NameAr : branch?.Currency?.NameEn
            };
            return DTO;
        }
        public static Branch? Map(BranchDTO? BranchDTO)
        {
            if (BranchDTO == null) return null;
            var _branch = new Branch()
            {
                Id = BranchDTO.Id,
                CompanyId = BranchDTO.CompanyId,
                BranchName = BranchDTO.BranchName,
                AddedDate = BranchDTO.AddedDate,
                Address = BranchDTO.Address,
                AddUserId = BranchDTO.AddUserId,
                IsActive = BranchDTO.IsActive,
                CurrencyId = BranchDTO.CurrencyId,
                CountryId = BranchDTO.CountryId,
                Email = BranchDTO.Email,
                IsMainBranch = BranchDTO.IsMainBranch,
                PhoneNumber = BranchDTO.PhoneNumber,
                TaxRegistrationNumber = BranchDTO.TaxRegistrationNumber,
                CommercialRecordNumber = BranchDTO.CommercialRecordNumber
            };
            return _branch;
        }

        public static List<BranchDTO?>? Map(List<Branch>? branches)
        {
            if (branches == null) return null;
            return branches.Select(selector: Map).ToList();
        }
    }
}
