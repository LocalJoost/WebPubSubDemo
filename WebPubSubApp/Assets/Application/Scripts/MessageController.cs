using System.Collections.Generic;
using LocalJoost.Service.WebPubSub.Interfaces;
using MRTKExtensions.Services.WebPubSub;
using Newtonsoft.Json;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;

namespace WebPubSubDemo
{
    public class MessageController : MonoBehaviour
    {
        private IWebPubSubClientService webPubSubClientService;
        
        [SerializeField]
        private List<ObjectController> objectControllers = new List<ObjectController>();

        private const string groupName = "testgroup";
        
        private void Start()
        {
            webPubSubClientService = ServiceManager.Instance.GetService<IWebPubSubClientService>();
            webPubSubClientService.OnMessageReceived.AddListener(OnMessageReceived);
            foreach( var objectController in objectControllers)
            {
                objectController.OnObjectSelected.AddListener(OnObjectSelected);
            }
        }
        
        private void OnMessageReceived(PubSubMessage msg)
        {
            if (msg is EventMessage { Event: "connected" })
            {
                webPubSubClientService.JoinGroup(groupName);
            }
            
            if(msg is GroupMessage groupMessage)
            {
                var objectData = JsonConvert.DeserializeObject<ObjectData>(groupMessage.Data.ToString());
                OnObjectSelected(objectData.Id, true);
            }
        }

        private void OnObjectSelected(int id)
        {
            OnObjectSelected(id, false);
        }
        
        private void OnObjectSelected(int id, bool isEventFromServer)
        {
            foreach (var objectController in objectControllers)
            {
                objectController.SelectById(id);
            }

            if (!isEventFromServer)
            {
                webPubSubClientService.SendMessage( groupName,new ObjectData{Id = id});
            }
        }
    }
}