using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine;

namespace MRTKExtensions.Service.WebPubSub
{
    [CreateAssetMenu(menuName = "WebPubSubClientServiceProfile", fileName = "WebPubSubClientServiceProfile",
        order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class WebPubSubClientServiceProfile : BaseServiceProfile<IServiceModule>
    {
        [SerializeField] 
        private string connectionLoadString;

        public string ConnectionLoadString => connectionLoadString;
    }
}
