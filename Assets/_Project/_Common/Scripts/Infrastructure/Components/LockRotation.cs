using UnityEngine;

namespace _Project._Common.Scripts.Infrastructure.Components
{
    public class LockRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
