using UnityEngine;

namespace Helpers
{
    public class LockRotation : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
