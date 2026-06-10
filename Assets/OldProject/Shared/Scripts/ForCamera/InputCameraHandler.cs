using Project.Shared.Scripts.Extensions;
using SaintsField;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Shared.Scripts.ForCamera
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InputCameraHandler : MonoBehaviour, IDragHandler
    {
        [SerializeField, ReadOnly, GetComponent(typeof(BoxCollider2D))]
        private BoxCollider2D boxCollider;
        [SerializeField] private CameraHandler cameraHandler;

        private void OnValidate()
        {
            var camBounds = cameraHandler.Camera.GetBounds();
            boxCollider.size = camBounds.size;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            cameraHandler.SetFreePosition(cameraHandler.Camera.ScreenToWorldPoint(eventData.position));
        }
    }
}