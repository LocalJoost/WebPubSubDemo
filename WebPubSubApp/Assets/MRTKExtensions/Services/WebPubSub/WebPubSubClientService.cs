using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LocalJoost.Service.WebPubSub.Interfaces;
using MRTKExtensions.Service.WebPubSub;
using MRTKExtensions.Services.WebPubSub;
using Newtonsoft.Json;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;
using UnityEngine.Events;

namespace LocalJoost.Service.WebPubSub
{
    [System.Runtime.InteropServices.Guid("dd04e16f-0626-4089-b1aa-21c36a2ba2db")]
    public class WebPubSubClientService : BaseServiceWithConstructor, IWebPubSubClientService
    {
        private readonly WebPubSubClientServiceProfile profile;
        private ClientWebSocket webSocket;
        private int ack = 1;
        private bool isListening = true;
        
        public UnityEvent<PubSubMessage> OnMessageReceived { get; } = new UnityEvent<PubSubMessage>();

        public WebPubSubClientService(string name, uint priority, WebPubSubClientServiceProfile profile)
            : base(name, priority)
        {
            this.profile = profile;
        }

        public override void Initialize()
        {
            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                var user = "tempUser";
                var paramDelimiter = profile.ConnectionLoadString.Contains("?") ? "&" : "?";
                var request = new HttpRequestMessage(HttpMethod.Get, 
                    $"{profile.ConnectionLoadString}{paramDelimiter}user={user}");
                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(request);
                    var result = await response.Content.ReadAsStringAsync();
                    await SetupWebSocketConnection(result);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private async Task SetupWebSocketConnection(string url)
        {
            webSocket = new ClientWebSocket();
            webSocket.Options.AddSubProtocol("json.webpubsub.azure.v1");
            await webSocket.ConnectAsync(new Uri(url), CancellationToken.None);
            Debug.Log("WebPubSubConnected");
            ListenForMessages(); // intentionally not awaited
        }

        private async Task ListenForMessages()
        {
            Debug.Log("WebPubSubConnected listening");
            while (isListening)
            {
                var buffer = new ArraySegment<byte>(new byte[1024]);
                var receiveResult = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                var result = Encoding.UTF8.GetString(buffer.Array, 0, receiveResult.Count);
                var message = PubSubMessage.FromJson(result);
                Debug.Log($"WebPubSubConnected received {message}");
                OnMessageReceived.Invoke(message);
            }
        }

        public void JoinGroup(string groupName)
        {
            var message = new
            {
                type = "joinGroup",
                group = groupName,
                ackId = ack++
            };
            SendMessageInternal(message);
        }

        public void LeaveGroup(string groupName)
        {
            var message = new
            {
                type = "leaveGroup",
                group = groupName,
                ackId = ack++
            };
            SendMessageInternal(message);
        }

        public void SendMessage(string groupName, object payload)
        {
            var message = new
            {
                type = "sendToGroup",
                group = groupName,
                ackId = ack++,
                noEcho = true,
                dataType = "json",
                data = payload
            };
            SendMessageInternal(message);
        }

        private void SendMessageInternal(object message)
        {
            if (webSocket is { State: WebSocketState.Open })
            {
                Debug.Log($"WebPubSubConnected sending {message}");
                var json = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                var buffer = new ArraySegment<byte>(bytes);
                webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public override void Destroy()
        {
            isListening = false;
            base.Destroy();
        }
    }
}