using SaintsField;
using UnityEngine;

namespace Project.Core.Player.Scripts
{
    [RequireComponent(typeof(SlingshotHandler))]
    public class SlingshotVisualHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly, GetComponent(typeof(SlingshotHandler))] 
        private SlingshotHandler slingshotHandler;
        
        [Space]
        [SerializeField] private Transform pointTransform;
        [SerializeField] private LineRenderer line;
        [SerializeField] private LineRenderer direct;
        [Space]
        [SerializeField] private float minWidth = 0.4f;
        [SerializeField] private float directHeight = 2f;

        private void SetActive(bool b)
        {
            if (pointTransform)
                pointTransform.gameObject.SetActive(b);
            
            if (direct)
                direct.gameObject.SetActive(b);
        }

        public void Show()
        {
            UpdatePosition();
            SetActive(true);
        }
        public void Hide() => SetActive(false);
        public void ResetPosition()
        {
            if (pointTransform)
                pointTransform.position = Vector2.zero;

            if (direct)
                direct.SetPosition(0, Vector2.zero);
            
            if (line)
            {
                for (int i = 0; i < line.positionCount; i++)
                    line.SetPosition(i, Vector3.zero);
            }
        }
        public void UpdatePosition()
        {
            Vector2 position = slingshotHandler.Position;
            var delta = slingshotHandler.CurrentDelta;

            if (pointTransform)
                pointTransform.position = position - delta;

            if (direct)
                direct.SetPosition(0, delta.normalized * directHeight);
        
            if (line)
            {
                var width = Mathf.Clamp(1f - (slingshotHandler.CurrentStrength - minWidth), 0f, 1f);
                var curve = line.widthCurve;
                var keys = curve.keys;
                keys[1].value = width;
                curve.SetKeys(keys);
                line.widthCurve = curve;
            
                var pos = -delta + delta.normalized * 0.25f;
                for (int i = 0; i < line.positionCount; i++)
                {
                    float t = i / (line.positionCount - 1f);
                    float centered = t - 0.5f;
                    float sign = Mathf.Sign(centered);
                    float absPowered = Mathf.Pow(Mathf.Abs(centered) * 2f, 3f);
                    float tNew = 0.5f + sign * absPowered / 2f;
                    line.SetPosition(i, Vector3.Lerp(Vector3.zero, pos, tNew));
                }
            }
        }
    }
}