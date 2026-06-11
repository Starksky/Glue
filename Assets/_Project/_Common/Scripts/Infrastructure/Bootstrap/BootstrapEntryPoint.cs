using System.Threading;
using _Project._Common.Scripts.Contracts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace _Project._Common.Scripts.Infrastructure.Bootstrap
{
    public class BootstrapEntryPoint : IAsyncStartable
    {
        private readonly string _startScene;
        private readonly ISceneLoaderService _sceneLoaderService;
        
        public BootstrapEntryPoint(string startScene, ISceneLoaderService sceneLoaderService)
        {
            _startScene = startScene;
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            //await UniTask.Delay(3000, cancellationToken: cancellation);
            
            var activeScene = SceneManager.GetActiveScene();
            
            if (activeScene.buildIndex == 0)
                await _sceneLoaderService.LoadSceneAsync(_startScene);
            else 
                await _sceneLoaderService.LoadSceneAsync(activeScene.name);
        }
    }
}