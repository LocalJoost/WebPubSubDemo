using Newtonsoft.Json;

namespace MRTKExtensions.Services.WebPubSub
{
    public class PubSubMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        public static PubSubMessage FromJson(string json)
        {
            var msgType = JsonConvert.DeserializeObject<PubSubMessage>(json);
            if (msgType != null)
            {
                return msgType.Type switch
                {
                    "message" => JsonConvert.DeserializeObject<GroupMessage>(json),
                    "system" => JsonConvert.DeserializeObject<EventMessage>(json),
                    "ack" => JsonConvert.DeserializeObject<AckMessage>(json),
                    _ => null
                };
            }

            return null;
        }
    }
}