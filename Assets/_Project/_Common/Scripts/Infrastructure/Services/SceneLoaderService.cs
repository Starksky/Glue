using _Project.Scripts.Contracts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace _Project._Common.Scripts.Infrastructure.Services
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly ILoaderScreen _loaderScreen;
        private string _currentScene;
        
        [Inject]
        public SceneLoaderService(ILoaderScreen loaderScreen)
        {
            _loaderScreen = loaderScreen;
        }
        
        public async UniTask LoadSceneAsync(string sceneName)
        {
            _loaderScreen.FadeIn();
            
            if (!string.IsNullOrWhiteSpace(_currentScene))
                await SceneManager.UnloadSceneAsync(_currentScene);
            
            await UniTask.Delay(1000);
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            
            _currentScene = sceneName;
            _loaderScreen.FadeOut();
        }

        public async UniTask ReloadCurrentScene()
        {
            if (string.IsNullOrWhiteSpace(_currentScene))
                return;
            
            _loaderScreen.FadeIn(); 
            
            var sceneToReload = _currentScene;
            
            await SceneManager.UnloadSceneAsync(sceneToReload);
            await UniTask.Delay(1000);
            await SceneManager.LoadSceneAsync(sceneToReload, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToReload));
            
            _loaderScreen.FadeOut();
        }
    }
}