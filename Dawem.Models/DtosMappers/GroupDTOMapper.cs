using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Core;

namespace Dawem.Models.DtosMappers
{
    public class GroupDTOMapper
    {
        private static RequestHeaderContext? userContext;

        public static void InitGroupContext(RequestHeaderContext _userContext)
        {
            userContext = _userContext;
        }
        public static GroupDTO? Map(Group? group)
        {
            if (group == null) return null;
            var DTO = new GroupDTO()
            {
                Id = group.Id,
                NameAr = group.NameAr,
                NameEn = group.NameEn,
                AddedDate = group.AddedDate,
                GlobalName = userContext?.Lang == "ar" ? group?.NameAr : group?.NameEn,
                MainBranchId = group.MainBranchId,
                IsActive = group.IsActive
            };
            return DTO;
        }
        public static GroupInfo? MapInfo(Group? group)
        {
            if (group == null) return null;
            var DTO = new GroupInfo()
            {
                Id = group.Id,
                GlobalName = userContext?.Lang == "ar" ? group?.NameAr : group?.NameEn,
                NameAr = group.NameAr,
                NameEn = group.NameEn,
                MainBranchId = group.MainBranchId,
                AddedDate = group.AddedDate,
                IsActive = group.IsActive
            };
            return DTO;
        }



        public static Group? Map(GroupDTO? groupDTO)
        {
            if (groupDTO == null) return null;
            var _group = new Group()
            {
                Id = groupDTO.Id,
                NameAr = groupDTO.NameAr,
                NameEn = groupDTO.NameEn,
                MainBranchId = groupDTO.MainBranchId,
                IsActive = groupDTO.IsActive
            };
            return _group;
        }

        public static List<GroupDTO?>? Map(List<Group?>? groups)
        {
            if (groups == null) return null;
            return groups.Select(selector: Map).ToList();
        }

    }
}
