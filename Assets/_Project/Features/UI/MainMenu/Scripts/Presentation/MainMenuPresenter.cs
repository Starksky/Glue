using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.UI.MainMenu.Scripts.Contracts;
using Cysharp.Threading.Tasks;

namespace _Project.Features.UI.MainMenu.Scripts.Presentation
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