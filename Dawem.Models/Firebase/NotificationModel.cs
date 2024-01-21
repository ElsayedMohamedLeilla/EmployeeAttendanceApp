using Dawem.Enums.Generals;
using Newtonsoft.Json;

namespace Dawem.Models.Firebase
{
    public class NotificationModel
    {
        [JsonProperty("deviseType")]
        public ApplicationType DeviceType { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        public List<TokensModel> Tokens { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }


}
