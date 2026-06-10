using UnityEngine;

namespace _Project.Scripts.Infrastructure.SoftBody
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class BonePhysics : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private BonePhysics[] distanceJoints;
        [SerializeField] private BonePhysics[] springJoints;
        [SerializeField] private BonePhysics[] hingeJoints;

        [Header("Физика кости")] 
        [SerializeField] private RigidbodyConstraints2D constraints = RigidbodyConstraints2D.FreezeRotation;
        [SerializeField] private float mass = 0.1f;
        [SerializeField] private float drag = 0.8f;
        [SerializeField] private float angularDrag = 0.5f;
        [SerializeField] private float gravityScale = 1f;

        [Header("Настройки соединения")] 
        [SerializeField] private bool autoConfigureConnectedAnchor = true;
        [SerializeField] private bool autoConfigureDistance = true;
        [SerializeField] private float jointFrequency = 10f;   // Жёсткость пружины
        [SerializeField] private float jointDamping = 2f;// Демпфирование
        [SerializeField] private bool useLimits = true;
        [SerializeField] private float lowerAngle = 0f;
        [SerializeField] private float upperAngle = 15f;
    
        [Header("Отталкивание от мира")]
        [SerializeField] private float collisionRadius = 0.15f;

        private Vector3 _localDefaultPosition;
        private Quaternion _quaternion;
        private CircleCollider2D col;
        private SpringJoint2D jointToParent;

        public Rigidbody2D Rigidbody => rb;
    
        void Awake()
        {
            _localDefaultPosition = transform.localPosition;
            _quaternion = transform.localRotation;
            col = GetComponent<CircleCollider2D>();

            if (!rb)
                rb = GetComponent<Rigidbody2D>();
            if (!rb)
                rb = gameObject.AddComponent<Rigidbody2D>();
        
            ConfigurePhysics();
        }

        private void Start()
        {
            ApplyJoints();
        }

        private void OnEnable()
        {
            rb.angularVelocity = 0f;
            rb.linearVelocity = Vector2.zero;
            transform.localPosition = _localDefaultPosition;
        }

        private void ConfigurePhysics()
        {
            rb.mass = mass;
            rb.linearDamping = drag;
            rb.angularDamping = angularDrag;
            rb.gravityScale = gravityScale;
            rb.constraints = constraints;
        
            col.radius = collisionRadius;
        }

        private void ApplyJoints()
        {
            foreach (var bone in distanceJoints)
            {
                if (bone == this)
                    continue;
            
                var spring = gameObject.AddComponent<DistanceJoint2D>();
                spring.connectedBody = bone.rb;
                spring.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
                spring.autoConfigureDistance = autoConfigureDistance;
            }
        
            foreach (var bone in springJoints)
            {
                if (bone == this)
                    continue;
            
                var spring = gameObject.AddComponent<SpringJoint2D>();
                spring.connectedBody = bone.rb;
                spring.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
                spring.autoConfigureDistance = autoConfigureDistance;
                spring.frequency = jointFrequency;
                spring.dampingRatio = jointDamping;
            }
        
            foreach (var bone in hingeJoints)
            {
                if (bone == this)
                    continue;
            
                var spring = gameObject.AddComponent<HingeJoint2D>();
                spring.connectedBody = bone.rb;
                spring.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            
                var limits = spring.limits;
                limits.max = upperAngle;
                limits.min = lowerAngle;
                spring.limits = limits;
                spring.useLimits = useLimits;
            }
        }

   
    
        /// <summary>
        /// Создать пружину к родительской кости
        /// </summary>
        public void CreateJointToParent(Transform parentBone)
        {
            if (parentBone == null) return;
        
            Rigidbody2D parentRb = parentBone.GetComponent<Rigidbody2D>();
            if (parentRb == null) return;
        
            jointToParent = gameObject.AddComponent<SpringJoint2D>();
            jointToParent.connectedBody = parentRb;
            //jointToParent.autoConfigureConnectedAnchor = true;
            jointToParent.autoConfigureDistance = true;
            jointToParent.frequency = jointFrequency;
            jointToParent.dampingRatio = jointDamping;
        }
    
        /// <summary>
        /// Применить силу к кости
        /// </summary>
        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
        {
            rb.AddForce(force, mode);
        }
    
        /// <summary>
        /// Сбросить скорость
        /// </summary>
        public void ResetVelocity()
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

        public Vector2 GetPosition() => transform.position;
    
        public Vector2 GetTowardCenter()
        {
            var lp = (Vector2)transform.localPosition;
            return (Vector2.zero - lp).normalized;
        }
        public Vector2 GetTowardLocalPoint()
        {
            var lp = (Vector2)transform.localPosition;
            var toward = (Vector2.zero - lp).normalized;
            return lp - toward * col.radius;
        }
    }
}