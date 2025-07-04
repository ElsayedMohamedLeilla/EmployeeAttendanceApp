﻿using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Requests.Tasks
{
    public class GetTaskDayModel
    {
        public int Day { get; set; }
        public WeekDay WeekDay { get; set; }
        public string WeekDayName { get; set; }
        public List<GetTaskModel> Tasks { get; set; }
    }
}
