using _Project.Scripts.Contracts.Interfaces;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Presentation.UI
{
    public class MainMenuPresenter : IMainMenuPresenter
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        
        public MainMenuPresenter(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask StartNewGame(string sceneName)
        {
            await _sceneLoaderService.LoadSceneAsync(sceneName);
        }
    }
}