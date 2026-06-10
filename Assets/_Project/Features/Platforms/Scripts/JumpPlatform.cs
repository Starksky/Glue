using DG.Tweening;
using SaintsField;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Core.Platforms.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpPlatform : MonoBehaviour
    {
        public enum ETypeJump
        {
            horizontal,
            vertical
        }
        
        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))]
        private Rigidbody2D rigidbody;

        [SerializeField] private ETypeJump typeJump;
        [SerializeField] private float distance;
        [SerializeField] private float jumpDuration = 1f;
        [SerializeField] private float returnDuration = 0.2f;
        [SerializeField] private UnityEvent EventReady;
        [SerializeField] private UnityEvent EventComplete;
        
        private Vector3 _startPos;
        
        private void Awake()
        {
            _startPos = transform.position;
        }
        
        public void Jump()
        {
            Sequence launchSequence = DOTween.Sequence();

            launchSequence.Append(rigidbody.DOMove(_startPos + transform.up * distance, jumpDuration));
            launchSequence.OnComplete(() =>
            {
                EventComplete.Invoke();
                
                Sequence localSequence = DOTween.Sequence();
                localSequence.Append(rigidbody.DOMove(_startPos, returnDuration));
                localSequence.OnComplete(() => EventReady.Invoke());
            });
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.red;
                var point = transform.position + transform.up * distance;
                Gizmos.DrawWireSphere(point, 0.5f);
                Gizmos.DrawLine(transform.position, point);
            }
        }
    }
}