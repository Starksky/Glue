using DG.Tweening;
using SaintsField;
using UnityEngine;

namespace _Project.Features.Platforms.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform : MonoBehaviour
    {
        public enum MovementType
        {
            UpDown,      // Вверх-вниз
            LeftRight,   // Влево-вправо
            Orbit,       // Вращение вокруг точки
            RotateSelf   // Вращение вокруг себя
        }

        [SerializeField, ReadOnly, GetComponent(typeof(Rigidbody2D))] private Rigidbody2D rb;
    
        [Header("Movement Settings")]
        [SerializeField] private MovementType type = MovementType.UpDown;
        [SerializeField] private float speed = 2f;           // Скорость движения
        [SerializeField] private float distance = 3f; // Амплитуда/Радиус
        [SerializeField] private float delay = 1f;
    
        [Header("Orbit Settings")]
        [SerializeField] private Transform orbitCenter;      // Центр для орбиты
        [SerializeField] private bool rotateToFaceCenter = false; // Поворачиваться лицом к центру
    
        [Header("Rotation Settings")]
        [SerializeField] private float rotationDegPerSecond = 90f;   // Градусов в секунду


        private Vector3 startPosition;
        private Vector3 endPosition;
        private Tween activeTween;
        private Tween activeTweenRotate;
        private bool movingForward = true;
    
        private void Awake()
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
            startPosition = transform.position;
        
            // Рассчитываем конечную позицию для линейных движений
            switch (type)
            {
                case MovementType.UpDown:
                    endPosition = startPosition + Vector3.up * distance;
                    break;
                case MovementType.LeftRight:
                    endPosition = startPosition + Vector3.right * distance;
                    break;
            }
        }

        private void Start()
        {
            StartMovement();
        }

        private void StartMovement()
        {
            activeTween?.Kill();
            activeTweenRotate?.Kill();
        
            activeTween = null;
            activeTweenRotate = null;
        
            switch (type)
            {
                case MovementType.UpDown:
                case MovementType.LeftRight:
                    StartLinearMovement();
                    break;
                case MovementType.Orbit:
                    StartOrbitMovement();
                    break;
                case MovementType.RotateSelf:
                    StartSelfRotation();
                    break;
            }
        }

        private void StartLinearMovement()
        {
            var sequence = DOTween.Sequence();
            float duration = distance / speed;
        
            // Бесконечное движение туда-обратно
            activeTween = sequence.Append(rb.DOMove(endPosition, duration).SetEase(Ease.Linear))
                .AppendInterval(delay)
                .Append(rb.DOMove(startPosition, duration).SetEase(Ease.Linear))
                .AppendInterval(delay)
                .SetLoops(-1)
                .SetUpdate(UpdateType.Fixed);
        }

        private void StartOrbitMovement()
        {
            if (orbitCenter == null)
            {
                Debug.LogWarning("Orbit Center не установлен! Использую стартовую позицию - (Vector3.zero)");
                orbitCenter = new GameObject($"{name}_OrbitCenter").transform;
                orbitCenter.position = startPosition;
            }
        
            float orbitDuration = (distance * 2 * Mathf.PI) / speed;
        
            if (rotateToFaceCenter)
            {
                // Создаём твин для вращения с обновлением позиции
                activeTweenRotate = DOTween.To(() => 0f, x => UpdateOrbitPosition(x), 360f, orbitDuration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart)
                    .SetUpdate(UpdateType.Fixed);
            }
            else
            {
                // Просто вращаем трансформ
                Vector3 orbitAxis = new Vector3(0, 0, 1);
                activeTween = rb.DORotate(360f, orbitDuration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart);
            
                // Двигаем по окружности отдельным твином
                UpdateOrbitPosition(0f);
            
                activeTweenRotate = DOTween.To(() => 0f, x => UpdateOrbitPosition(x), 360f, orbitDuration)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart)
                    .SetUpdate(UpdateType.Fixed);
            }
        }

        private void UpdateOrbitPosition(float angle)
        {
            Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * distance;
            rb.MovePosition(orbitCenter.position + offset);
        
            if (rotateToFaceCenter)
            {
                Vector3 direction = (orbitCenter.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                rb.SetRotation(Quaternion.Euler(0, 0, targetAngle));
            } else rb.SetRotation(Quaternion.Euler(0, 0, 0));
        }

        private void StartSelfRotation()
        {
            activeTweenRotate = rb.DORotate(360f, 360f / rotationDegPerSecond)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(UpdateType.Fixed);
        }
    

        private void OnDestroy()
        {
            activeTween?.Kill();
            activeTweenRotate?.Kill();
        }

#if UNITY_EDITOR
        // Визуализация в редакторе
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                startPosition = transform.position;
                if (type == MovementType.UpDown)
                    endPosition = startPosition + Vector3.up * distance;
                else if (type == MovementType.LeftRight)
                    endPosition = startPosition + Vector3.right * distance;
            }
        
            Gizmos.color = Color.green;
            switch (type)
            {
                case MovementType.UpDown:
                    Gizmos.DrawLine(startPosition, endPosition);
                    Gizmos.DrawWireSphere(startPosition, 0.2f);
                    Gizmos.DrawWireSphere(endPosition, 0.2f);
                    break;
                case MovementType.LeftRight:
                    Gizmos.DrawLine(startPosition, endPosition);
                    Gizmos.DrawWireSphere(startPosition, 0.2f);
                    Gizmos.DrawWireSphere(endPosition, 0.2f);
                    break;
                case MovementType.Orbit:
                    Gizmos.color = Color.blue;
                    if (orbitCenter != null)
                    {
                        UnityEditor.Handles.DrawWireDisc(orbitCenter.position, Vector3.forward, distance);
                    }
                    else if (Application.isPlaying)
                    {
                        UnityEditor.Handles.DrawWireDisc(startPosition, Vector3.forward, distance);
                    }
                    break;
                case MovementType.RotateSelf:
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position, 0.5f);
                    break;
            }
        }
#endif
    
    }
}