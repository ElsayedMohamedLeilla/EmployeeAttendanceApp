using Dawem.Domain.Entities.Others;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others;

namespace Dawem.Models.DtosMappers.Others
{
    public class ActionLogDTOMapper
    {
        private static RequestInfo? userContext;

        public static void InitActionLogContext(RequestInfo _userContext)
        {
            userContext = _userContext;
        }
        public static ActionLogDTO? Map(ScreenPermissionLog? actionLog)
        {
            if (actionLog == null) return null;
            var DTO = new ActionLogDTO()
            {
                Id = actionLog.Id,
                ActionType = actionLog.ActionType,
                //ActionPlace = actionLog.ActionPlace,
                //BranchId = actionLog.BranchId,
                ResponseStatus = actionLog.ResponseStatus
            };
            return DTO;
        }
        public static ActionLogInfo? MapInfo(ScreenPermissionLog? actionLog)
        {
            if (actionLog == null) return null;
            var DTO = new ActionLogInfo()
            {
                Id = actionLog.Id,
                Date = actionLog.Date,
                //BranchId = actionLog.BranchId,
                UserId = actionLog.UserId,
                ActionType = actionLog.ActionType,
                //ActionPlace = actionLog.ActionPlace,
                //BranchGlobalName = actionLog?.Branch?.Name,
                UserGlobalName = actionLog?.User?.Name,
                ResponseStatus = actionLog.ResponseStatus
            };
            return DTO;
        }
        public static ScreenPermissionLog? Map(ActionLogInfo? actionLogDTO)
        {
            if (actionLogDTO == null) return null;
            var _actionLog = new ScreenPermissionLog()
            {
                Id = actionLogDTO.Id,
                Date = actionLogDTO.Date,
                //BranchId = actionLogDTO.BranchId,
                UserId = actionLogDTO.UserId,
                ActionType = actionLogDTO.ActionType,
                //ActionPlace = actionLogDTO.ActionPlace,
                ResponseStatus = actionLogDTO.ResponseStatus
            };
            return _actionLog;
        }
        public static List<ActionLogDTO?>? Map(List<ScreenPermissionLog?>? actionLogs)
        {
            if (actionLogs == null) return null;
            return actionLogs.Select(selector: Map).ToList();
        }

    }
}
