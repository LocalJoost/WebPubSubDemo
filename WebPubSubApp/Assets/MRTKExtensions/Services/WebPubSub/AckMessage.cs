using Newtonsoft.Json;

namespace MRTKExtensions.Services.WebPubSub
{
    public class AckMessage : PubSubMessage
    {
        [JsonProperty("ackId")]
        public int AckId { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
        
        public override string ToString()
        {
            return $"{GetType().Name} - AckId: {AckId} Success: {Success}";
        }
    }
}