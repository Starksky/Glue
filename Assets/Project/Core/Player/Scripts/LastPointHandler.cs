using Cysharp.Threading.Tasks;
using UnityEngine;

public class LastPointHandler : MonoBehaviour
{
    [SerializeField] private GameObject skin;
    [SerializeField] private SlingshotController slingshotController;
    [SerializeField] private Transform respawnPoint;
    
    public async void ToLastPoint()
    {
        skin.gameObject.SetActive(false);
        slingshotController.ResetPosition(respawnPoint.position);
        await UniTask.Yield();
        skin.gameObject.SetActive(true);
    }
}
