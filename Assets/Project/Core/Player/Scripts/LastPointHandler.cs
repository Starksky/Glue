using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class LastPointHandler : MonoBehaviour
{
    [SerializeField] private GameObject skin;
    [FormerlySerializedAs("slingshotController")] [SerializeField] private OldSlingshotController oldSlingshotController;
    [SerializeField] private Transform respawnPoint;
    
    public async void ToLastPoint()
    {
        skin.gameObject.SetActive(false);
        oldSlingshotController.ResetPosition(respawnPoint.position);
        await UniTask.Yield();
        skin.gameObject.SetActive(true);
    }
}
