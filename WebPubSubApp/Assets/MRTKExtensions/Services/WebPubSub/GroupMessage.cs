using Newtonsoft.Json;

namespace MRTKExtensions.Services.WebPubSub
{
    public class GroupMessage : PubSubMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("fromUserId")]
        public string FromUserId { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name} - from {From} ({FromUserId}) to {Group}: {Data}";
        }
    }
}