using UnityEngine;

namespace _Project.Scripts.Utils
{
    public class LockRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
