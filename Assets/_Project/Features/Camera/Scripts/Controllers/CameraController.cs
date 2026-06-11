using _Project._Common.Scripts.Contracts;
using _Project._Common.Scripts.Utils;
using SaintsField;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace _Project.Features.Camera.Scripts.Controllers
{
    public class CameraController : MonoBehaviour, IDragHandler
    {
        [SerializeField, ReadOnly, GetComponent(typeof(BoxCollider2D))]
        private BoxCollider2D boxCollider;
        
        private ICameraView _cameraView;
        
        [Inject]
        public void Construct(ICameraView cameraView)
        {
            _cameraView = cameraView;
        }
        
        private void OnValidate()
        {
            var camBounds = UnityEngine.Camera.main.GetBounds();
            boxCollider.size = camBounds.size;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _cameraView.SetFreeScreenPosition(eventData.position);
        }
    }
}