using Newtonsoft.Json;

namespace MRTKExtensions.Services.WebPubSub
{
    public class EventMessage : PubSubMessage
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name} - Event: {Event}, UserId: {UserId}, ConnectionId: {ConnectionId}";
        }
    }
}