
using MRTKExtensions.Services.WebPubSub;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine.Events;

namespace LocalJoost.Service.WebPubSub.Interfaces
{
    public interface IWebPubSubClientService : IService
    {
        void JoinGroup(string groupName);
        void LeaveGroup(string groupName);
        void SendMessage(string groupName, object message);
        
        UnityEvent<PubSubMessage> OnMessageReceived { get; }
    }
}