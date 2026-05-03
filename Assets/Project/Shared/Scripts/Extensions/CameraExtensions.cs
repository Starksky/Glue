using UnityEngine;

namespace Project.Shared.Scripts.Extensions
{
    public static class CameraExtensions
    {
        public static Bounds GetBounds(this Camera camera)
        {
            if (camera == null || !camera.orthographic)
            {
                Debug.LogWarning("The camera is not orthographic or has not been found!");
                return new Bounds();
            }

            float height = camera.orthographicSize * 2f;
            float width = height * camera.aspect;
    
            Vector3 cameraCenter = camera.transform.position;
            Bounds cameraBounds = new Bounds(cameraCenter, new Vector3(width, height, 0));

            return cameraBounds;
        }
    }
}