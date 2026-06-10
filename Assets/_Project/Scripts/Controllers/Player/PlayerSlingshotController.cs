using _Project.Scripts.Contracts.Interfaces;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace _Project.Scripts.Controllers.Player
{
    public class PlayerSlingshotController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ISessionService<ICameraView> _cameraSessionService;
        private IPlayerSlingshotVisualView _playerSlingshotVisualView;
        private IPlayerSlingshotView _playerSlingshotView;
        private IPlayerBodyView _playerBodyView;
        private ICameraView _cameraView;
        
        [Inject]
        public void Construct(
            IPlayerBodyView playerBodyView,
            IPlayerSlingshotView playerSlingshotView,
            IPlayerSlingshotVisualView  playerSlingshotVisualView,
            ISessionService<ICameraView> cameraSessionService)
        {
            _playerBodyView =  playerBodyView;
            _playerSlingshotVisualView = playerSlingshotVisualView;
            _cameraSessionService = cameraSessionService;
            _playerSlingshotView = playerSlingshotView;
        }

        private void Awake()
        {
            _cameraSessionService.Session
                .Subscribe(session => _cameraView = session)
                .RegisterTo(destroyCancellationToken);
            
            _playerSlingshotVisualView.Hide();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_playerBodyView.HasContactWithSurface)
                return;
            
            _playerSlingshotVisualView.ResetPosition();
            _playerSlingshotView.OnBeginDrag();
            _playerSlingshotVisualView.Show();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!_playerBodyView.HasContactWithSurface)
                return;
            
            _playerSlingshotView.OnStayDrag(_cameraView.ScreenToWorldPoint(eventData.position));
            _playerSlingshotVisualView.UpdatePosition();
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_playerBodyView.HasContactWithSurface)
                return;
            
            _playerSlingshotView.OnEndDrag();
            _playerSlingshotVisualView.ResetPosition();
            _playerSlingshotVisualView.Hide();
        }
    }
}