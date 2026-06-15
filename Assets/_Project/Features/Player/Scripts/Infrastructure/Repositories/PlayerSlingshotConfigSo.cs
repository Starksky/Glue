using _Project.Features.Player.Scripts.Contracts;
using UnityEngine;

namespace _Project.Features.Player.Scripts.Infrastructure.Repositories
{
    [CreateAssetMenu(fileName = "SlingshotConfig", menuName = "Create/SlingshotConfig", order = 0)]
    public class PlayerSlingshotConfigSo : ScriptableObject, IPlayerSlingshotConfig
    {
        [SerializeField] private float maxDragDistanceBody = 0.25f;
        [SerializeField] private float maxDragDistance = 1.8f;
        [SerializeField] private float maxForce = 8f;
        [SerializeField] private float angleForForce = 45f;
        [SerializeField] private float dragForce = 10f;
        
        public float MaxDragDistanceBody => maxDragDistanceBody;
        public float MaxDragDistance => maxDragDistance;
        public float ThrowForce => maxForce;
        public float AngleForForce => angleForForce;
        public float DragForce => dragForce;
    }
}