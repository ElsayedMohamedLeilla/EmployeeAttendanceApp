using Dawem.Domain.Entities.Ohters;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others;

namespace Dawem.Models.DtosMappers.Others
{
    public class ActionLogDTOMapper
    {
        private static RequestHeaderContext? userContext;

        public static void InitActionLogContext(RequestHeaderContext _userContext)
        {
            userContext = _userContext;
        }
        public static ActionLogDTO? Map(ActionLog? actionLog)
        {
            if (actionLog == null) return null;
            var DTO = new ActionLogDTO()
            {
                Id = actionLog.Id,
                ActionType = actionLog.ActionType,
                ActionPlace = actionLog.ActionPlace,
                BranchId = actionLog.BranchId,
                ResponseStatus = actionLog.ResponseStatus
            };
            return DTO;
        }
        public static ActionLogInfo? MapInfo(ActionLog? actionLog)
        {
            if (actionLog == null) return null;
            var DTO = new ActionLogInfo()
            {
                Id = actionLog.Id,
                Date = actionLog.Date,
                BranchId = actionLog.BranchId,
                UserId = actionLog.UserId,
                ActionType = actionLog.ActionType,
                ActionPlace = actionLog.ActionPlace,
                BranchGlobalName = actionLog?.Branch?.Name,
                UserGlobalName = actionLog?.User?.FirstName + " " + actionLog?.User?.FirstName,
                ResponseStatus = actionLog.ResponseStatus
            };
            return DTO;
        }
        public static ActionLog? Map(ActionLogInfo? actionLogDTO)
        {
            if (actionLogDTO == null) return null;
            var _actionLog = new ActionLog()
            {
                Id = actionLogDTO.Id,
                Date = actionLogDTO.Date,
                BranchId = actionLogDTO.BranchId,
                UserId = actionLogDTO.UserId,
                ActionType = actionLogDTO.ActionType,
                ActionPlace = actionLogDTO.ActionPlace,
                ResponseStatus = actionLogDTO.ResponseStatus
            };
            return _actionLog;
        }
        public static List<ActionLogDTO?>? Map(List<ActionLog?>? actionLogs)
        {
            if (actionLogs == null) return null;
            return actionLogs.Select(selector: Map).ToList();
        }

    }
}
