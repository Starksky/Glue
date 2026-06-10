using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Contracts.Interfaces
{
    public interface ISceneLoaderService
    {
        UniTask LoadSceneAsync(string sceneName);
        UniTask ReloadCurrentScene();
    }
}