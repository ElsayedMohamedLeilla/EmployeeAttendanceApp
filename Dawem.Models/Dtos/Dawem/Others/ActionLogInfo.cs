﻿using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class ActionLogInfo : ActionLogDTO
    {
        public ActionLogInfo()
        {
        }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public string UserGlobalName { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}