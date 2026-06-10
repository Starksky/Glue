using SaintsField;
using SaintsField.Playa;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Project.Core.Player.Scripts
{
    [RequireComponent(typeof(PlayerInput),
        typeof(SlingshotHandler))]
    public class SlingshotInputHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(PlayerInput))]
        private PlayerInput playerInput;
        [SerializeField, ReadOnly, GetComponent(typeof(SlingshotHandler))]
        private SlingshotHandler slingshotHandler;

        [SerializeField] private Collider2D dragCollider2D;

        [LayoutStart("Events", ELayout.Foldout)]
        [SerializeField] private UnityEvent<Vector2> EventBeginDrag;
        [SerializeField] private UnityEvent<Vector2> EventUpdateDrag;
        [SerializeField] private UnityEvent<Vector2> EventEndDrag;

        private Bounds _boundsClick;
        private Camera _camera;
        private InputAction _dragAction;
        private InputAction _dragPositionAction;
        private bool _isDragging;

        private void OnEnable()
        {
            _boundsClick.extents = Vector3.one * 2f;
            _camera = Camera.main;
            _dragAction = playerInput.actions["Drag"];
            _dragPositionAction = playerInput.actions["DragPosition"];
            
            if (_dragAction != null)
            {
                _dragAction.started += OnDragStarted;
                _dragAction.canceled += OnDragCanceled;
                _dragAction.Enable();
            }
        
            if (_dragPositionAction != null)
                _dragPositionAction.Enable();
        }

        private void OnDisable()
        {
            if (_dragAction != null)
            {
                _dragAction.started -= OnDragStarted;
                _dragAction.canceled -= OnDragCanceled;
                _dragAction.Disable();
            }
        
            if (_dragPositionAction != null)
                _dragPositionAction.Disable();
        }

        private void LateUpdate()
        {
            if (_isDragging)
            {
                var positionMouse = GetMousePosition();
                slingshotHandler.UpdateDrag(positionMouse);
                EventUpdateDrag.Invoke(positionMouse);
            }
        }

        private Vector2 GetMousePosition()
        {
            Vector2 screenPos = _dragPositionAction.ReadValue<Vector2>();
            return _camera.ScreenToWorldPoint(screenPos);
        }

        private void OnDragStarted(InputAction.CallbackContext context)
        {
            _boundsClick.center = transform.position;
            
            var mousePosition = GetMousePosition();
            if (slingshotHandler.IsFlying || !dragCollider2D.OverlapPoint(mousePosition))
                return;
            
            _isDragging = true;
            slingshotHandler.StartDrag();
            EventBeginDrag.Invoke(mousePosition);
        }

        private void OnDragCanceled(InputAction.CallbackContext context)
        {
            _isDragging = false;
            slingshotHandler.EndDrag();
            EventEndDrag.Invoke(GetMousePosition());
        }
    }
}