using System;
using Project.Shared.Scripts.Extensions;
using SaintsField;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Shared.Scripts.ForCamera
{
    [RequireComponent(typeof(Camera))]
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(Camera))] private Camera camera;
        [SerializeField] private Grid map;
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        [SerializeField] private bool isDebugBounds;

        private Vector3? _freePosition;
        private Bounds _mapBounds;
        private Bounds _cameraBounds;

        public Camera Camera => camera;
        
        private void Awake()
        {
            _mapBounds = map.GetBounds();
            _cameraBounds = camera.GetBounds();
        }
        public void ToTarget() => _freePosition = null;
        public void SetFreePosition(Vector3? position)
        {
            _freePosition = position;
        }

        public void OnDrawGizmos()
        {
            if (!isDebugBounds)
                return;
            
            _mapBounds = map.GetBounds();
            _cameraBounds = camera.GetBounds();
            _cameraBounds.center = transform.position;
            _mapBounds.DebugDrawBounds(Color.green);
            _cameraBounds.DebugDrawBounds(Color.blue);
        }

        private void FixedUpdate()
        {
            if (!target && _freePosition == null)
                return;
            
            var position = transform.position;
            var targetPosition = _freePosition ?? target.position;
            targetPosition.z = position.z;
            
            if ((position - targetPosition).magnitude < 0.1f)
                return;
            
            position = Vector3.Lerp(position, targetPosition, Time.deltaTime * speed);
            _cameraBounds.center = position;
            position = _mapBounds.ClosestBoundInBounds(_cameraBounds);
            transform.position = position;
        }
    }
}