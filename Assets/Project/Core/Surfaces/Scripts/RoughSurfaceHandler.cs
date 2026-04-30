using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Shared.Scripts.ForMessages;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace Project.Core.Surfaces.Scripts
{
    public class RoughSurfaceHandler : BaseMonoMessage
    {
        [SerializeField] private float delayAction;
        [SerializeField] private SignalAsset message;
        
        private CancellationTokenSource _cancellationTokenSource;

        [Inject] private MessageBrokersService _messageBrokersService;
        private bool CanExecute => _cancellationTokenSource == null;
        
        public void OnExecute(Collider2D col)
        {
            if (!CanExecute)
                return;
            
            StartExecute().Forget();
        }

        public void OnCancel()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async UniTask StartExecute()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            
            try
            {
                await UniTask.WaitForSeconds(delayAction, cancellationToken: _cancellationTokenSource.Token);
                _messageBrokersService.Publish(Chanel, message);
            }
            catch (Exception)
            {
                //ignore
            }
            
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}