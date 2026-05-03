using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Shared.Scripts.ForCamera
{
    public class InputCameraHandler : MonoBehaviour, IDragHandler
    {
        [SerializeField] private CameraHandler _cameraHandler;
        //private Vector3 _lastPosition;
        
        public void OnDrag(PointerEventData eventData)
        {
            //_lastPosition += -_cameraHandler.Camera.ScreenToWorldPoint(eventData.position);
            _cameraHandler.SetFreePosition(_cameraHandler.Camera.ScreenToWorldPoint(eventData.position));
        }
    }
}