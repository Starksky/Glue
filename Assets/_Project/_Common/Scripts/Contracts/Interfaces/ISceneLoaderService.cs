using Cysharp.Threading.Tasks;

namespace _Project._Common.Scripts.Contracts.Interfaces
{
    public interface ISceneLoaderService
    {
        UniTask LoadSceneAsync(string sceneName);
        UniTask ReloadCurrentScene();
    }
}