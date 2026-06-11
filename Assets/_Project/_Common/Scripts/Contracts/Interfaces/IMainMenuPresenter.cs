using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Contracts.Interfaces
{
    public interface IMainMenuPresenter
    {
        UniTask StartNewGame(string sceneName);
    }
}