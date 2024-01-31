using Newtonsoft.Json;

namespace Dawem.Models.RealTime.Firebase
{
    public class NotificationModel
    {

        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        public List<string> Tokens { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public string ImageUrl { get; set; }


    }


}
