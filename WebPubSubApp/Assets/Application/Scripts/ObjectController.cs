using MixedReality.Toolkit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace WebPubSubDemo
{
    public class ObjectController : MRTKBaseInteractable
    {
        [SerializeField]
        private int objectId = -1;
        private Color baseColor;
        private Material objMaterial;
        
        public UnityEvent<int> OnObjectSelected { get; } = new UnityEvent<int>();

        private void Start()
        {
            objMaterial = GetComponent<Renderer>().material;
            baseColor = objMaterial.color;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
           // objMaterial.color = Color.red;
            OnObjectSelected?.Invoke(objectId);
        }

        public void SelectById(int id)
        {
            objMaterial.color = id == objectId ? Color.red : baseColor;
        }
    }
}