﻿using Newtonsoft.Json;

namespace Dawem.Models.DTOs.Dawem.RealTime.Firebase
{
    public class ResponseModel
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
