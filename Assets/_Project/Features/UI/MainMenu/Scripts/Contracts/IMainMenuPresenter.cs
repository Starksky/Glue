using Cysharp.Threading.Tasks;

namespace _Project.Features.UI.MainMenu.Scripts.Contracts
{
    public interface IMainMenuPresenter
    {
        UniTask StartNewGame(string sceneName);
    }
}