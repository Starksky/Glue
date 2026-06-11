using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Player.Scripts.Contracts;
using UnityEngine;
using VContainer;

namespace _Project.Features.Player.Scripts.View
{
    public class PlayerSlingshotView : MonoBehaviour, IPlayerSlingshotView
    {
        private IPlayerSlingshotPresenter _playerSlingshotPresenter;
        private IPlayerBodyView _playerBodyView;
        private bool _isDragging;
        
        [Inject]
        public void Construct(IPlayerBodyView playerBodyView, 
            IPlayerSlingshotPresenter playerSlingshotPresenter)
        {
            _playerBodyView = playerBodyView;
            _playerSlingshotPresenter = playerSlingshotPresenter;
        }

        public Vector2 Position => _playerSlingshotPresenter.Position;
        public Vector2 DeltaDrag => _playerSlingshotPresenter.DeltaDrag;
        public float StrengthDrag => _playerSlingshotPresenter.StrengthDrag;
        
        public void OnBeginDrag()
        {
            _playerSlingshotPresenter.BeginDrag();
        }
        
        public void OnStayDrag(Vector2 positionMouse)
        {
            _playerSlingshotPresenter.StayDrag(positionMouse, _playerBodyView.ContactClosestPoint ?? transform.position);
        }

        public void OnEndDrag()
        {
            _playerSlingshotPresenter.EndDrag(_playerBodyView.ContactClosestPoint ?? transform.position);
        }
    }
}