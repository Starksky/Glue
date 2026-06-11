using _Project.Features.Player.Scripts.Contracts;
using UnityEngine;
using VContainer;

namespace _Project.Features.Player.Scripts.View
{
    public class PlayerSlingshotVisualView : MonoBehaviour, IPlayerSlingshotVisualView
    {
        [SerializeField] private Transform pointTransform;
        [SerializeField] private LineRenderer line;
        [SerializeField] private LineRenderer direct;
        
        private IPlayerSlingshotView _playerSlingshotView;
        private IPlayerSlingshotVisualConfig _config;
        
        [Inject]
        public void Construct(IPlayerSlingshotView playerSlingshotView,
            IPlayerSlingshotVisualConfig config)
        {
            _playerSlingshotView = playerSlingshotView;
            _config = config;
        }
        
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
            Vector2 position = _playerSlingshotView.Position;
            var delta = _playerSlingshotView.DeltaDrag;

            if (pointTransform)
                pointTransform.position = position - delta;

            if (direct)
                direct.SetPosition(0, delta.normalized * _config.DirectHeight);
        
            if (line)
            {
                var width = Mathf.Clamp(1f - (_playerSlingshotView.StrengthDrag - _config.MinWidth), 0f, 1f);
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